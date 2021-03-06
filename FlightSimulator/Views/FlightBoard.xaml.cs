﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for MazeBoard.xaml
    /// </summary>
    public partial class FlightBoard : UserControl
    {

        ObservableDataSource<Point> planeLocations = null;
        private FlightBoardViewModel flightBoardView;
        public delegate void Del(object sender, PropertyChangedEventArgs e) ;
        public FlightBoard()
        {
            InitializeComponent();
            flightBoardView = new FlightBoardViewModel();
            // rigister the get notify from the view model
            flightBoardView.PropertyChanged += Vm_PropertyChanged;
            DataContext = flightBoardView;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            planeLocations = new ObservableDataSource<Point>();
            // Set identity mapping of point in collection to point on plot
            planeLocations.SetXYMapping(p => p);

            plotter.AddLineGraph(planeLocations, 2, "Route");
        }

        private void Vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Nullable<float> y = flightBoardView.Lon;
            Nullable<float> x = flightBoardView.Lat;
            // if lon and lat changed add points the flight board 
            if ((e.PropertyName.Equals("Lat") || e.PropertyName.Equals("Lon")) && (x != null && y != null))
            {
                Point p1 = new Point((float)x, (float)y);
                planeLocations.AppendAsync(Dispatcher, p1);
            }
        }
    }
}

