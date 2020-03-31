using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartSearch.Abstractions;
using SmartSearch.LuceneNet.Tests.Mocks;
using System.Linq;
using FluentAssertions;
using System;

namespace SmartSearch.LuceneNet.Tests
{
    



    public partial class FilterTests
    {
        private const string LocationField = "Geolocation";
        private const string LocationNameField = "GeolocationName";

        [TestMethod]
        public void LatLngFilterWorks()
        {
            var env = TestEnvironment.Build();

            TestLocations(env, LatLngFilter.CreateRadiusInKm(LocationField, -25.428595, -49.258507, 1),
                "Impact Hub Curitiba");

            TestLocations(env, LatLngFilter.CreateRadiusInKm(LocationField, -25.4285549, -49.25781069999999, 10),
                "Impact Hub Curitiba", "Parque Barigui");

            TestLocations(env, LatLngFilter.CreateRadiusInKm(LocationField, -25.429596, -49.271271, 10),
                "Impact Hub Curitiba", "Parque Barigui");

            TestLocations(env, LatLngFilter.CreateBox(LocationField, new LatLng(-25.573163, -49.368852), new LatLng(-25.353702, -49.181480)),
                "Impact Hub Curitiba", "Parque Barigui");

            TestLocations(env, LatLngFilter.CreateBox(LocationField, new LatLng(44.477891, 5.110339), new LatLng(50.692108, 15.345120)),
                "Geneva Switzerland", "Lausanne Switzerland");

            TestLocations(env, LatLngFilter.CreateBox(LocationField, new LatLng(37.809352, -122.521424), new LatLng(37.608588, -122.279663)),
                "San Francisco US");

            TestLocations(env, LatLngFilter.CreateBox(LocationField, new LatLng(34.134542, -16.374786), new LatLng(-32.722599, 50.392885))
                /* no results expected */);
        }
    }
}