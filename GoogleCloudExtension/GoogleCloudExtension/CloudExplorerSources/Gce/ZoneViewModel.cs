﻿// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using GoogleCloudExtension.CloudExplorer;
using GoogleCloudExtension.DataSources;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoogleCloudExtension.CloudExplorerSources.Gce
{
    public class ZoneViewModel : TreeHierarchy, ICloudExplorerItemSource
    {
        private const string IconResourcePath = "CloudExplorerSources/Gce/Resources/zone_icon.png";

        private static readonly Lazy<ImageSource> s_zoneIcon = new Lazy<ImageSource>(() => ResourceUtils.LoadImage(IconResourcePath));
        private static readonly TreeLeaf s_noInstancesPlaceholder = new TreeLeaf
        {
            Caption = Resources.CloudExplorerGceNoInstancesInZoneCaption,
            IsWarning = true
        };
        private static readonly TreeLeaf s_noWindowsInstancesPlaceholder = new TreeLeaf
        {
            Caption = Resources.CloudExplorerGceNoWindowsInstancesInZoneCaption,
            IsWarning = true
        };

        private readonly GceSourceRootViewModel _owner;
        private readonly InstancesPerZone _instancesPerZone;

        public event EventHandler ItemChanged;

        public object Item => new ZoneItem(_instancesPerZone.Zone);

        public ZoneViewModel(GceSourceRootViewModel owner, InstancesPerZone instancesPerZone, bool onlyWindowsInstances)
        {
            _owner = owner;
            _instancesPerZone = instancesPerZone;

            var instancesToShow = instancesPerZone.Instances.Where(x => !onlyWindowsInstances || x.IsWindowsInstance()).ToList();

            Caption = $"{instancesPerZone.Zone.Name} ({instancesToShow.Count})";
            Icon = s_zoneIcon.Value;

            var viewModels = instancesToShow.Select(x => new GceInstanceViewModel(owner, x));
            foreach (var viewModel in viewModels)
            {
                Children.Add(viewModel);
            }

            var menuItems = new List<MenuItem>
            {
                new MenuItem { Header = Resources.CloudExplorerGceNewInstanceMenuHeader, Command = new WeakCommand(OnNewInstanceCommand) },
                new MenuItem { Header = Resources.UiPropertiesMenuHeader, Command = new WeakCommand(OnPropertiesCommand) },
            };
            ContextMenu = new ContextMenu { ItemsSource = menuItems };

            if (Children.Count == 0)
            {
                if (onlyWindowsInstances)
                {
                    Children.Add(s_noWindowsInstancesPlaceholder);
                }
                else
                {
                    Children.Add(s_noInstancesPlaceholder);
                }
            }
        }

        private void OnPropertiesCommand()
        {
            _owner.Context.ShowPropertiesWindow(Item);
        }

        private void OnNewInstanceCommand()
        {
            var url = $"https://console.cloud.google.com/compute/instancesAdd?project={_owner.Context.CurrentProject.Name}&zone={_instancesPerZone.Zone.Name}";
            Process.Start(url);
        }
    }
}