using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CNN_1
{
    public partial class Form1 : Form
    {
        private Controller maincontr;

        public Form1()
        {
            InitializeComponent();
            setupTriggers();
            maincontr = new Controller("C:\\Users\\ryv26\\source\\repos\\CNN 1\\Data.json");
            initsComboBox();
        }

        private void initsComboBox()
        {
            var genres = maincontr.GetUniqueGenres();
            var actors = maincontr.GetUniqueActors();
            var ratings = maincontr.GetUniqueRatings();
            var durations = maincontr.GetUniqueDurations();

            foreach (var genre in genres)
            {
                comboBox1.Items.Add(genre);
            }
            foreach (var actor in actors)
            {
                comboBox4.Items.Add(actor);
            }
            foreach (var rating in ratings)
            {
                comboBox2.Items.Add(rating.ToString() + " Звезд и более");
            }
            foreach (var duration in durations)
            {
                comboBox3.Items.Add(duration.ToString() + " минут");
            }
        }
        private void setupTriggers()
        {
            comboBox1.SelectedIndexChanged += TriggerHandler;
            comboBox4.SelectedIndexChanged += TriggerHandler;
            comboBox2.SelectedIndexChanged += TriggerHandler;
            comboBox3.SelectedIndexChanged += TriggerHandler;
        }
        private bool isTriggerActive = false;
        private void TriggerHandler(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (isTriggerActive) return;
            isTriggerActive = true;
            try
            {
                string selectedGenre = comboBox1.SelectedItem?.ToString() ?? "Не выбрано";
                string selectedActor = comboBox4.SelectedItem?.ToString() ?? "Не выбрано";
                string selectedRating = comboBox2.SelectedItem?.ToString() ?? "Не выбрано";
                string selectedDuration = comboBox3.SelectedItem?.ToString() ?? "Не выбрано";

                bool isActorSelected = checkBox1.Checked;  
                bool isDurationSelected = checkBox2.Checked;  

                int? minRating = selectedRating != "Не выбрано" ? (int?)int.Parse(selectedRating.Split(' ')[0]) : null;

                int? maxDuration = isDurationSelected && selectedDuration != "Не выбрано" ? (int?)int.Parse(selectedDuration.Split(' ')[0]) : null;

                string actorFilter = isActorSelected && selectedActor != "Не выбрано" ? selectedActor : null;

                string result = Authmesange(selectedGenre, actorFilter ?? "Не выбрано", selectedRating, selectedDuration);

                listBox1.Items.Add(result);

                var filteredMovies = maincontr.GetMovies(selectedGenre, minRating, maxDuration, actorFilter);

                if (filteredMovies.Count > 0)
                {
                    listBox1.Items.Add("Подходящие фильмы:\n");
                    foreach (var movie in filteredMovies)
                    {
                        listBox1.Items.Add($"{movie.Title}");
                        listBox1.Items.Add($"Жанр: {movie.Genre}, Рейтинг: {movie.Rating}, Длительность: {movie.Duration} минут");
                    }
                }
                else
                {
                    listBox1.Items.Add("Нет фильмов, соответствующих заданным критериям.");
                }
            }
            finally
            {
                isTriggerActive = false;
            }
        }
        private string Authmesange(string selectedGenre, string selectedActor, string selectedRating, string selectedDuration)
        {
            StringBuilder message = new StringBuilder("Примененные фильтры:\n");

            if (selectedGenre != "Не выбрано")
                message.AppendLine($"Жанр: {selectedGenre}");
            if (selectedActor != "Не выбрано")
                message.AppendLine($"Актер: {selectedActor}");
            if (selectedRating != "Не выбрано")
                message.AppendLine($"Рейтинг: {selectedRating}");
            if (selectedDuration != "Не выбрано")
                message.AppendLine($"Длительность: {selectedDuration}");

            return message.ToString();
        }
    }
}
