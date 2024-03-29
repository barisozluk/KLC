﻿using DynamicData;
using ReactiveUI;
using AYP.Helpers.Enums;
using AYP.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Windows;
using AYP.Enums;

namespace AYP.ViewModel
{
    public partial class NodeViewModel
    {
        public ReactiveCommand<Unit, Unit> CommandUnSelectedAllConnectors { get; set; }
        public ReactiveCommand<Unit, Unit> CommandAddEmptyConnector { get; set; }
        public ReactiveCommand<SelectMode, Unit> CommandSelect { get; set; }
        public ReactiveCommand<Point, Unit> CommandMove { get; set; }
        public ReactiveCommand<(int index, ConnectorViewModel connector), Unit> CommandAddConnectorWithConnect { get; set; }
        public ReactiveCommand<ConnectorViewModel, Unit> CommandDeleteConnectorWithConnect { get; set; }
        public ReactiveCommand<string, Unit> CommandValidateName { get; set; }

        public ReactiveCommand<ConnectorViewModel, Unit> CommandSelectWithShiftForConnectors { get; set; }
        public ReactiveCommand<ConnectorViewModel, Unit> CommandSetConnectorAsStartSelect { get; set; }

        private void SetupCommands()
        {
            CommandSelect = ReactiveCommand.Create<SelectMode>(Select);
            CommandMove = ReactiveCommand.Create<Point>(Move);
            CommandAddEmptyConnector = ReactiveCommand.Create(AddEmptyConnector);
            CommandSelectWithShiftForConnectors = ReactiveCommand.Create<ConnectorViewModel>(SelectWithShiftForConnectors);
            CommandSetConnectorAsStartSelect = ReactiveCommand.Create<ConnectorViewModel>(SetConnectorAsStartSelect);
            CommandUnSelectedAllConnectors = ReactiveCommand.Create(UnSelectedAllConnectors);
            CommandAddConnectorWithConnect = ReactiveCommand.Create<(int index, ConnectorViewModel connector)>(AddConnectorWithConnect);
            CommandDeleteConnectorWithConnect = ReactiveCommand.Create<ConnectorViewModel>(DeleteConnectorWithConnec);
            CommandValidateName = ReactiveCommand.Create<string>(ValidateName);

            NotSavedSubscrube();
        }
        private void NotSavedSubscrube()
        {
            CommandMove.Subscribe(_ => NotSaved());
            CommandAddConnectorWithConnect.Subscribe(_ => NotSaved());
            CommandDeleteConnectorWithConnect.Subscribe(_ => NotSaved());
            CommandValidateName.Subscribe(_ => NotSaved());
        }
        private void NotSaved()
        {
            NodesCanvas.ItSaved = false;
        }

        public int GetConnectorIndex(ConnectorViewModel connector)
        {
            return Transitions.Items.IndexOf(connector);
        }

        private void AddConnectorWithConnect((int index, ConnectorViewModel connector) element)
        {
            //Transitions.Insert(element.index, element.connector);
            if (element.connector.Connect != null)
            {
                NodesCanvas.CommandAddConnect.ExecuteWithSubscribe(element.connector.Connect);
            }
        }
        private void DeleteConnectorWithConnec(ConnectorViewModel connector)
        {
            if (connector.Connect != null)
            {
                NodesCanvas.CommandDeleteConnect.ExecuteWithSubscribe(connector.Connect);
            }
            //Transitions.Remove(connector);
        }
        private void Select(SelectMode selectMode)
        {
            if (selectMode == SelectMode.ClickWithCtrl)
            {
                this.Selected = !this.Selected;
                return;
            }
            else if ((selectMode == SelectMode.Click) && (!Selected))
            {
                NodesCanvas.CommandUnSelectAll.ExecuteWithSubscribe();
                this.Selected = true;
            }
        }
        private void Move(Point delta)
        {
            //Point moveValue = delta.Division(NodesCanvas.Scale.Value);
            Point1 = Point1.Addition(delta);
        }


        private void ValidateName(string newName)
        {

            NodesCanvas.CommandValidateNodeName.ExecuteWithSubscribe((this, newName));
        }


        public void AddEmptyConnector()
        {        
            foreach(var output in OutputList)
            {
                CurrentConnector = new ConnectorViewModel(NodesCanvas, this, output.Name, output.PositionConnectPoint, output.UniqueId,
                    output.KapasiteId, output.MinKapasite, output.MaxKapasite, output.FizikselOrtamId, output.GerilimTipiId, output.KullanimAmaciId,
                    output.GirdiDuraganGerilimDegeri1, output.GirdiDuraganGerilimDegeri2, output.GirdiDuraganGerilimDegeri3,
                    output.GirdiMinimumGerilimDegeri, output.GirdiMaksimumGerilimDegeri, output.GirdiTukettigiGucMiktari, output.CiktiDuraganGerilimDegeri,
                    output.CiktiUrettigiGucKapasitesi, output.Label, output.TypeId, output.Id, output.Port)
                {
                    TextEnable = true,
                    Visible = true
                };

                if (CurrentConnector.TypeId == (int)TipEnum.GucUreticiGucArayuzu && CurrentConnector.KullanimAmaciId == (int)KullanimAmaciEnum.Cikti)
                {
                    CurrentConnector.KalanKapasite = output.KalanKapasite;
                }
                else
                {
                    CurrentConnector.KalanKapasite = null;
                }
                CurrentConnector.AgAkisList = output.AgAkisList;

                Transitions.Add(CurrentConnector);
            }

            CurrentConnector = null;
        }
        private void UnSelectedAllConnectors()
        {
            foreach (var transition in Transitions.Items)
            {
                transition.Selected = false;
            }

            IndexStartSelectConnectors = 0;
        }
        private void SetConnectorAsStartSelect(ConnectorViewModel viewModelConnector)
        {
            IndexStartSelectConnectors = Transitions.Items.IndexOf(viewModelConnector) - 1;
        }
        private void SelectWithShiftForConnectors(ConnectorViewModel viewModelConnector)
        {
            if (viewModelConnector == null)
                return;

            var transitions = this.Transitions.Items.Skip(1);
            int indexCurrent = transitions.IndexOf(viewModelConnector);
            int indexStart = IndexStartSelectConnectors;
            UnSelectedAllConnectors();
            IndexStartSelectConnectors = indexStart;
            transitions = transitions.Skip(Math.Min(indexCurrent, indexStart)).SkipLast(Transitions.Count - Math.Max(indexCurrent, indexStart) - 2);
            foreach (var transition in transitions)
            {
                transition.Selected = true;
            }
        }
    }
}
