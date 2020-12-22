using ChemSharp.Molecules.DataProviders;
using ChemSharp.Molecules.Math;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ChemSharp.Molecules
{
    public class Molecule : INotifyPropertyChanged
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
        /// The Molecules Bonds
        /// </summary>
        public ObservableCollection<Bond> Bonds { get; set; }

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
        /// <param name="bonds"></param>
        public Molecule(IEnumerable<Atom> atoms, IEnumerable<Bond> bonds = null) : this()
        {
            Atoms = new ObservableCollection<Atom>(atoms);
            if (bonds != null) Bonds = new ObservableCollection<Bond>(bonds);
        }

        /// <summary>
        /// creates Molecule with ObservableCollection of Atoms
        /// </summary>
        /// <param name="atoms"></param>
        /// <param name="bonds"></param>
        public Molecule(ObservableCollection<Atom> atoms, ObservableCollection<Bond> bonds = null) : this()
        {
            Atoms = atoms;
            if (bonds != null) Bonds = bonds;
        }


        /// <summary>
        /// When DataProvider is changed, add data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AtomDataProvider):
                    Atoms = new ObservableCollection<Atom>(_atomDataProvider.Atoms);
                    break;
                case nameof(BondDataProvider):
                    Bonds = new ObservableCollection<Bond>(_bondDataProvider.Bonds);
                    break;
            }
        }

        private IAtomDataProvider _atomDataProvider;
        ///<summary>
        /// Provides AtomData
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

        private IBondDataProvider _bondDataProvider;
        ///<summary>
        /// Provides BondData
        /// </summary>
        public IBondDataProvider BondDataProvider
        {
            get => _bondDataProvider;
            set
            {
                _bondDataProvider = value;
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
