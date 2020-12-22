﻿using ChemSharp.Extensions;
using ChemSharp.Files;
using ChemSharp.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Molecules.DataProviders
{
    public class CIFDataProvider : IAtomDataProvider, IBondDataProvider
    {
        /// <summary>
        /// import recipes
        /// </summary>
        static CIFDataProvider()
        {
            if (!FileHandler.RecipeDictionary.ContainsKey("cif"))
                FileHandler.RecipeDictionary.Add("cif", s => new PlainFile<string>(s));
        }

        public CIFDataProvider(string path)
        {
            var file = (PlainFile<string>)FileHandler.Handle(path);
            var loops = Loops(file.Content);
            var cellLengths = CellParameters(file.Content, "cell_length").ToArray();
            var cellAngles = CellParameters(file.Content, "cell_angle").ToArray();
            var moleculeLoop = Array.Find(loops, s => s.Contains("_atom_site_label")).DefaultSplit();
            var bondLoop = Array.Find(loops, s => s.Contains("_geom_bond")).DefaultSplit();
            var conversionMatrix = FractionalCoordinates.ConversionMatrix(cellLengths[0], cellLengths[1],
                cellLengths[2], cellAngles[0], cellAngles[1], cellAngles[2]);
            Atoms = ReadAtoms(moleculeLoop, conversionMatrix);
            Bonds = ReadBonds(bondLoop);
        }


        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IEnumerable<Atom> Atoms { get; set; }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IEnumerable<Bond> Bonds { get; set; }

        /// <summary>
        /// Create loop data
        /// </summary>
        /// <returns></returns>
        private string[] Loops(string[] data)
        {
            var text = string.Join('\n', data);
            return text.Split(new[] { "loop_" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Reads in Atoms and converts fractional to cartesian coordinates
        /// </summary>
        /// <param name="moleculeLoop"></param>
        /// <param name="conversionMatrix"></param>
        /// <returns></returns>
        private IEnumerable<Atom> ReadAtoms(string[] moleculeLoop, Vector3[] conversionMatrix)
        {
            var headers = moleculeLoop.Where(s => s.Trim().StartsWith("_")).ToArray();
            var disorderGroupIndex = Array.IndexOf(headers, "_atom_site_disorder_group");
            foreach (var line in moleculeLoop.Where(s => !s.StartsWith("_")))
            {
                var raw = line.Split(" ");
                //ignore disorder group
                if (disorderGroupIndex >= 0
                    && disorderGroupIndex < raw.Length
                    && raw[disorderGroupIndex] == "2"
                    || (raw.Length != headers.Length)) continue;
                var rawCoordinates = new Vector3(
                    raw[2].RemoveUncertainty().ToSingle(),
                    raw[3].RemoveUncertainty().ToSingle(),
                    raw[3].RemoveUncertainty().ToSingle());
                var coordinates = FractionalCoordinates.FractionalToCartesian(rawCoordinates, conversionMatrix);
                yield return new Atom(raw[1]) { Location = coordinates, Title = raw[0] };
            }
        }

        private IEnumerable<Bond> ReadBonds(string[] bondLoop)
        {
            var tmp = Atoms.ToDictionary(atom => atom.Title);
            foreach (var line in bondLoop.Where(s => !s.StartsWith("_")))
            {
                if (string.IsNullOrEmpty(line)) continue;
                var raw = line.Split(" ");
                if (raw.Length == 0) continue;
                var a1 = tmp.TryAndGet(raw[0]);
                var a2 = tmp.TryAndGet(raw[1]);
                if (a1 == null || a2 == null) continue;
                yield return new Bond(a1, a2);
            }
        }


        /// <summary>
        /// Returns cell parameters by given string array and param name
        /// </summary>
        /// <param name="input"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static IEnumerable<float> CellParameters(string[] input, string param) => input.Where(s => s.StartsWith($"_{param}"))
            .Select(s => s.Split(' ').Last().RemoveUncertainty().ToSingle());
    }
}
