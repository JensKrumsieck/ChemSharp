﻿using ChemSharp.Extensions;
using ChemSharp.Molecule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ChemSharp.Files.Molecule
{
    public class CIF : TextFile, IAtomFile
    {
        public CIF(string path) : base(path)
        {
            Atoms = ReadAtoms();
        }

        public IEnumerable<Atom> Atoms { get; set; }

        /// <summary>
        /// Read in Atoms from CIF
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Atom> ReadAtoms()
        {
            var cellLengths = CellParameters("cell_length").ToArray();
            var cellAngles = CellParameters("cell_angle").ToArray();
            var moleculeLoop = Loop("atom_site_label");
            var headers = moleculeLoop.LineSplit().Where(s => s.Trim().StartsWith("_")).ToArray();
            int disorderGroupIndex = Array.IndexOf(headers, "_atom_site_disorder_group");

            var conversionMatrix = MathUtil.ConversionMatrix(cellLengths[0], cellLengths[1], cellLengths[2],
                cellAngles[0], cellAngles[1], cellAngles[2]);

            //loop over all non header lines
            foreach (var line in moleculeLoop.LineSplit().Where(s => !s.StartsWith("_")))
            {
                var raw = line.Split(" ");
                //ignore disorder group
                if(disorderGroupIndex >= 0 && disorderGroupIndex < raw.Length && raw[disorderGroupIndex] == "2" || (raw.Length != headers.Length)) continue;
                
                var symbol = raw[1];
                var rawCoordinates = new Vector3(
                    raw[2].StripUncertainity().ToFloat(), 
                    raw[3].StripUncertainity().ToFloat(), 
                    raw[4].StripUncertainity().ToFloat());

                var coordinates = MathUtil.FractionalToCartesian(rawCoordinates, conversionMatrix);
                yield return new Atom(raw[1])
                {
                    Location = coordinates,
                    Title = raw[0]
                };
            }
        }

        /// <summary>
        /// extract cell parameters
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private IEnumerable<float> CellParameters(string param) => Data.Where(s => s.StartsWith($"_{param}"))
            .Select(s => s.Split(' ').Last().StripUncertainity().ToFloat());

        private string[] _loops { get; set; }

        /// <summary>
        /// Gets specified loop
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string Loop(string name)
        {
            _loops ??= Loops();
            return Array.Find(_loops, s => s.Contains($"_{name}"));
        }

        /// <summary>
        /// Create loop data
        /// </summary>
        /// <returns></returns>
        private string[] Loops()
        {
            var text = string.Join('\n', Data);
            return text.Split(new[] { "loop_" }, StringSplitOptions.None);
        }
    }
}
