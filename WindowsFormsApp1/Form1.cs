using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Globalization;
using System.Speech.Recognition;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine recognizer = null;
        SpeechSynthesizer synth = null;

        public Form1()
        {
            InitializeComponent();





            //- TTS -
            synth = new SpeechSynthesizer();

            // Configure the audio output.
            synth.SetOutputToDefaultAudioDevice();

            synth.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.NotSet, 0, new CultureInfo("es-ES"));

            // Speak a string.
            synth.Speak("Digues el nom d'un color");






            //- STT -
            recognizer = createSpeechEngine("es-ES");
            // Create a grammar.
            Choices colors = new Choices(new string[] { "blanc","gris","negre","groc","verd","blau","vermell","rosa","marró","lila","taronja" });
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = new CultureInfo("es-ES");
            gb.Append("Color"); // Cal dir: "Color x"
            gb.Append(colors);

            // Create a Grammar object and load it to the recognizer.
            Grammar g = new Grammar(gb);
            //g.Name = ("Color x");
            recognizer.LoadGrammarAsync(g);

            // Attach a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(testGrammar_SpeechRecognized);

            // Set the input to the recognizer.
            recognizer.SetInputToDefaultAudioDevice();

            // Start recognition.
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("Starting asynchronous speech recognition... ");

        }


        //Handle the grammar's SpeechRecognized event, output the recognized text.
        void testGrammar_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string strColor = "";
            Console.WriteLine("Recognized text: " + e.Result.Text + "Confidence: " + e.Result.Confidence);

            if (e.Result.Confidence < 0.65) return;

            try
            {
                strColor = e.Result.Text.Split(' ')[1];
            }
            catch { }

            switch (strColor)
            {
                case "blanc":
                    label1.Text = "Blanc";
                    synth.SpeakAsync("Blanc");
                    label1.BackColor = Color.White;
                    label1.ForeColor = Color.FromArgb(Color.White.ToArgb() ^ 0xffffff);
                    break;
                case "gris":
                    label1.Text = "Gris";
                    synth.SpeakAsync("Gris");
                    label1.BackColor = Color.Gray;
                    label1.ForeColor = Color.Black; //Color.FromArgb(Color.Gray.ToArgb() ^ 0xffffff);
                    break;
                case "negre":
                    label1.Text = "Negre";
                    synth.SpeakAsync("Negre");
                    label1.BackColor = Color.Black;
                    label1.ForeColor = Color.FromArgb(Color.Black.ToArgb() ^ 0xffffff);
                    break;
                case "groc":
                    label1.Text = "Groc";
                    synth.SpeakAsync("Groc");
                    label1.BackColor = Color.Yellow;
                    label1.ForeColor = Color.FromArgb(Color.Yellow.ToArgb() ^ 0xffffff);
                    break;
                case "verd":
                    label1.Text = "Verd";
                    synth.SpeakAsync("Verd");
                    label1.BackColor = Color.Lime;
                    label1.ForeColor = Color.FromArgb(Color.Lime.ToArgb() ^ 0xffffff);
                    break;
                case "blau":
                    label1.Text = "Blau";
                    synth.SpeakAsync("Blau");
                    label1.BackColor = Color.DodgerBlue;
                    label1.ForeColor = Color.FromArgb(Color.DodgerBlue.ToArgb() ^ 0xffffff);
                    break;
                case "vermell":
                    label1.Text = "Vermell";
                    synth.SpeakAsync("Vermell");
                    label1.BackColor = Color.Red;
                    label1.ForeColor = Color.FromArgb(Color.Red.ToArgb() ^ 0xffffff);
                    break;
                case "rosa":
                    label1.Text = "Rosa";
                    synth.SpeakAsync("Rosa");
                    label1.BackColor = Color.Pink;
                    label1.ForeColor = Color.FromArgb(Color.Pink.ToArgb() ^ 0xffffff);
                    break;
                case "marró":
                    label1.Text = "Marró";
                    synth.SpeakAsync("Marró");
                    label1.BackColor = Color.Brown;
                    label1.ForeColor = Color.FromArgb(Color.Brown.ToArgb() ^ 0xffffff);
                    break;
                case "lila":
                    label1.Text = "Lila";
                    synth.SpeakAsync("Lila");
                    label1.BackColor = Color.Orchid;
                    label1.ForeColor = Color.FromArgb(Color.Orchid.ToArgb() ^ 0xffffff);
                    break;
                case "taronja":
                    label1.Text = "Taronja";
                    synth.SpeakAsync("Taronja");
                    label1.BackColor = Color.Orange;
                    label1.ForeColor = Color.FromArgb(Color.Orange.ToArgb() ^ 0xffffff);
                    break;
                default:
                    label1.Text = "???";
                    break;
            }
        }


        private SpeechRecognitionEngine createSpeechEngine(string preferredCulture)
        {
            foreach (RecognizerInfo config in SpeechRecognitionEngine.InstalledRecognizers())
            {
                if (config.Culture.ToString() == preferredCulture)
                {
                    recognizer = new SpeechRecognitionEngine(config);
                    break;
                }
            }

            // if the desired culture is not found, then load default
            if (recognizer == null)
            {
                MessageBox.Show("The desired culture is not installed on this machine, the speech-engine will continue using "
                    + SpeechRecognitionEngine.InstalledRecognizers()[0].Culture.ToString() + " as the default culture.",
                    "Culture " + preferredCulture + " not found!");
                recognizer = new SpeechRecognitionEngine(SpeechRecognitionEngine.InstalledRecognizers()[0]);
            }

            return recognizer;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            recognizer.RecognizeAsyncStop();
            recognizer.Dispose();
            synth.Dispose();
        }
    }
}
