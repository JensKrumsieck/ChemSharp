using ChemSharp.Molecules.Math;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using ChemSharp.Molecules.DataProviders;

namespace ChemSharp.Molecules
{
    public class Molecule: INotifyPropertyChanged
    {
        /// <summary>
        /// Title for Molecule
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Molecules Atoms
        /// </summary>
        public ObservableCollection<Atom> Atoms { get; set; }

        /// <summary>
        /// creates Molecule without Atoms to add later
        /// </summary>
        public Molecule()
        {
            PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// creates Molecule with IEnumerable of Atoms
        /// </summary>
        /// <param name="atoms"></param>
        public Molecule(IEnumerable<Atom> atoms) :this() => Atoms = new ObservableCollection<Atom>(atoms);
        /// <summary>
        /// creates Molecule with ObservableCollection of Atoms
        /// </summary>
        /// <param name="atoms"></param>
        public Molecule(ObservableCollection<Atom> atoms) : this() => Atoms = atoms;

        /// <summary>
        /// When DataProvider is changed, add data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(AtomDataProvider)) return;
            Atoms = new ObservableCollection<Atom>(_atomDataProvider.Atoms);
        }

        private IAtomDataProvider _atomDataProvider;
        ///<summary>
        /// <inheritdoc />
        /// </summary>
        public IAtomDataProvider AtomDataProvider
        {
            get => _atomDataProvider;
            set
            {
                _atomDataProvider = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Wrapper for Centroid Method
        /// </summary>
        public Vector3 Centroid => Atoms.Centroid();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
