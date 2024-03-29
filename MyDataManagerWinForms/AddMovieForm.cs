﻿using DataLibrary;
using MyDataManagerDataOperations;
using MyDataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MyDataManagerWinForms.MainForm;

namespace MyDataManagerWinForms
{
    public partial class AddMovieForm : Form
    {
        public event PopulateMessageEvent populateMessageVariable;
        private Movie _movie;
        public AddMovieForm()
        {
            InitializeComponent();
        }

        public AddMovieForm(Movie movie)
        {
            InitializeComponent();
            _movie = movie;
            this.txtMovieId.Text = movie.Id.ToString();
            this.txtMovieTitle.Text = movie.Title;
            this.txtMovieYear.Text = movie.Year.ToString();
        }

        private void btnOkMovie_Click(object sender, EventArgs e)
        {
            var dataOps = new DataOperations();

            if (string.IsNullOrWhiteSpace(this.txtMovieId.Text))
            {
                //add
                if (string.IsNullOrWhiteSpace(this.txtMovieTitle.Text) || string.IsNullOrWhiteSpace(this.txtMovieYear.Text))
                {
                    MessageBox.Show("Enter an valid movie title.", "Missing Movie Title", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                }

                if (!int.TryParse(this.txtMovieYear.Text, out int newYear))
                {
                    MessageBox.Show("Enter a valid year", "Invalid Year", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                addNewMovie(this.txtMovieTitle.Text, newYear);
            }
            else
            {
                //update
                if (!int.TryParse(this.txtMovieYear.Text, out int newYear))
                {
                    MessageBox.Show("Enter a valid year");
                }
                else
                {
                    Task.Run(async () => await dataOps.updateMovie(this.txtMovieId.Text, this.txtMovieTitle.Text, newYear));

                    if (populateMessageVariable is not null)
                    {
                        populateMessageVariable($"{this.txtMovieId.Text} ({newYear}) updated");
                    }
                }
            }
            this.Close();
        }

        private void addNewMovie(string title, int year)
        {
            var newMovie = new Movie();
            newMovie.Title = title;
            newMovie.Year = year;
            var dataImport = new DataImporter();

            Task.Run(async () => await dataImport.GetNewMovie(newMovie));

            //if (populateMessageVariable is not null)
            //{
            //	populateMessageVariable($"{title} ({year}) added");
            //}

            MessageBox.Show($"{title} {year} is added or updated in the database", "Operation Completed",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnCancelMovie_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}