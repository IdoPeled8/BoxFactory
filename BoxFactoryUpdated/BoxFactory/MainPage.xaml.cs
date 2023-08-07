using FactoryLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BoxFactory
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Stock Manager;
        public MainPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Manager = e.Parameter as Stock;

            string msg = Manager.CheckDate();
            if (!string.IsNullOrEmpty(msg))
            {
                Helper.userMsgs(msg);
            }

            RefreshFactory();
            
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Manager.GetValid(addWidth.Text, addHeight.Text, addCount.Text, Navigate.AddBox);
                Helper.userMsgs(Helper.AddComplete);
            }
            catch (ArgumentException returnAmount)
            {
                Helper.userMsgs(returnAmount.Message);
            }
            catch (Exception unvalidNum)
            {
                Helper.userMsgs(unvalidNum.Message);
            }
            RefreshFactory();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshFactory();
            try
            {
                foreach (var item in Manager.GetValid(addWidth.Text, addHeight.Text, addCount.Text, Navigate.SearchBox))
                {
                    boxesFoundList.Items.Add(item.ToString());
                }
            }
            catch (ArgumentException nullBoxes)
            {
                Helper.userMsgs(nullBoxes.Message);
            }
            catch (Exception unvalidNum)
            {
                Helper.userMsgs(unvalidNum.Message);
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSelectedNullCheck())
            {
                try
                {
                    Box clickedBox = boxesMainList.SelectedItem as Box;
                    Manager.RemoveBox(clickedBox);
                    Helper.userMsgs($"{clickedBox} Removed from the Factory");
                }
                catch (Exception dosentExist)
                {
                    messageText.Text = dosentExist.Message;
                }
                RefreshFactory();
            }
            else
                Helper.userMsgs("didnt pick any box to remove");
        }

        private void buyButton_Click(object sender, RoutedEventArgs e)
        {
            if (boxesFoundList.Items.Count == 0)
            {
                return;
            }
            string removeBoxMsg = Manager.CheckOut();
            if (!string.IsNullOrEmpty(removeBoxMsg))
            {
                Helper.userMsgs($"these boxes Removed\n{removeBoxMsg}");
            }
            Helper.userMsgs("Thanks for buying");

            RefreshFactory();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (boxesFoundList.Items.Count == 0)
            {
                return;
            }
            Manager.shoppingCart.Clear();
            RefreshFactory();
        }
        public bool ListViewSelectedNullCheck()
        {
            if (boxesMainList.SelectedItem == null)
            {
                //ErrorMassages.ItemNotSelectedError();
                return false;
            }
            return true;
        }



        public void RefreshFactory()
        {
            boxesMainList.Items.Clear();
            boxesFoundList.Items.Clear();
            Manager.shoppingCart.Clear();
            messageText.Text = string.Empty;

            foreach (var item in Manager.Boxes)
            {
                boxesMainList.Items.Add(item);
            }

        }
    }
}
