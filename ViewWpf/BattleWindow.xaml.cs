﻿using Controllers;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ViewWpf
{
    /// <summary>
    /// Interaction logic for BattleWindow.xaml
    /// </summary>
    public partial class BattleWindow : Window
    {

        BattleController bat = new BattleController();

        public BattleWindow()
        {
            InitializeComponent();

            if (bat.initial_turn()== true)
            {
                ia_play();
            }
            
            //MessageBox.Show("Player jogada:" + bat.printturno());
        }


        private void Character1_red_skill1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //personagem selecionado

            Character1_blue_life.Content = bat.attack_red(Character1_blue_life.Content);
            if (bat.dead_confirmation(Character1_blue_life.Content) == true)
            {
                MessageBox.Show("persongem / time blue morreu");
                //muda img para morto
            }
        }

        private void Pass_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pass_turn();
            ia_play();
        }

        //Jogada da IA
        private void ia_play()
        {
            Character1_red_life.Content = bat.attack_blue(Character1_red_life.Content);
            pass_turn();
        }

        //Alterar numeração da label de turno
        private void pass_turn()
        {
            Turn.Content = bat.pass_turn(Turn.Content);
        }
    }
}