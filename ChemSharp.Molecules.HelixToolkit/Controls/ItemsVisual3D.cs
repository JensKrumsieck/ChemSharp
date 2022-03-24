using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

namespace ChemSharp.Molecules.HelixToolkit.Controls;

public class ItemsVisual3D : ModelVisual3D
{
	public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
	 "ItemsSource",
	 typeof(IEnumerable),
	 typeof(ItemsVisual3D),
	 new PropertyMetadata(null, (s, e) => ((ItemsVisual3D)s).ItemsSourceChanged(e)));

	public IEnumerable ItemsSource
	{
		get => (IEnumerable)GetValue(ItemsSourceProperty);
		set => SetValue(ItemsSourceProperty, value);
	}

	private void ItemsSourceChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.OldValue is INotifyCollectionChanged oldObservableCollection)
			oldObservableCollection.CollectionChanged -= OnCollectionChanged;

		if (e.NewValue is INotifyCollectionChanged observableCollection)
			observableCollection.CollectionChanged += OnCollectionChanged;

		if (ItemsSource == null) return;

		Children.Clear();
		AddItems(ItemsSource);
	}

	private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		switch (e.Action)
		{
			case NotifyCollectionChangedAction.Add:
				AddItems(e.NewItems);
				break;
			case NotifyCollectionChangedAction.Remove:
				RemoveItems(e.OldItems);
				break;
			case NotifyCollectionChangedAction.Replace:
				RemoveItems(e.OldItems);
				AddItems(e.NewItems);
				break;
			case NotifyCollectionChangedAction.Reset:
				Children.Clear();
				AddItems(ItemsSource);
				break;
		}
	}

	private void RemoveItems(IEnumerable items)
	{
		if (items == null) return;

		foreach (var item in items.Cast<Visual3D>()) Children.Remove(item);
	}

	private void AddItems(IEnumerable items)
	{
		if (items == null) return;

		foreach (var item in items.Cast<Visual3D>()) Children.Add(item);
	}
}
