using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MapaMir4
{
    public partial class Form1 : Form
    {
        SoundPlayer alarme = new SoundPlayer();

        // Vamos definir as cores aqui em cima para facilitar se quisermos mudar depois
        Color corDoPerigo = Color.Red;

        public Form1()
        {
            InitializeComponent();
            alarme.SoundLocation = @"C:\Windows\Media\tada.wav";
            alarme.Load();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Deactivate += (s, e) => this.ActiveControl = null;

            // =========================================================
            // REGISTAR TODOS OS BOSSES NA FÁBRICA ATUALIZADA:
            // Agora passamos a cor base (LimeGreen ou Gold)
            // =========================================================

            // Bosses Verdes (30 minutos, cor inicial LimeGreen)
            ConfigurarBoss(btnBossVerde1, lblTempoBossVerde1, 30, Color.LimeGreen);
            ConfigurarBoss(btnBossVerde2, lblTempoBossVerde2, 30, Color.LimeGreen);
            ConfigurarBoss(btnBossVerde3, lblTempoBossVerde3, 30, Color.LimeGreen);
            ConfigurarBoss(btnBossVerde4, lblTempoBossVerde4, 30, Color.LimeGreen);

            // Bosses Amarelos / Dourados (60 minutos, cor inicial Gold)
            ConfigurarBoss(btnBossAmarelo1, lblTempoBossAmarelo1, 60, Color.Gold);
            ConfigurarBoss(btnBossAmarelo2, lblTempoBossAmarelo2, 60, Color.Gold);
        }

        // ====================================================================
        // A NOSSA FÁBRICA DE CRONÔMETROS (Atualizada para o Visual B)
        // ====================================================================
        // Novo parâmetro: Color corOriginalBoss
        private void ConfigurarBoss(Button botao, Label texto, int minutos, Color corOriginalBoss)
        {
            // --- NOVA BLINDAGEM DO BOTÃO INVISÍVEL ---
            botao.TabStop = false; // Impede que o botão receba o "foco" do Windows
            botao.FlatStyle = FlatStyle.Flat; // Garante o estilo plano
            botao.FlatAppearance.BorderSize = 0; // Força a borda zero
            botao.FlatAppearance.MouseDownBackColor = Color.Transparent; // Remove a cor ao clicar
            botao.FlatAppearance.MouseOverBackColor = Color.Transparent; // Remove a cor ao passar o rato
            // -----------------------------------------

            texto.BackColor = Color.Transparent;
            texto.ForeColor = corOriginalBoss;

            // --- GARANTIA VISUAL (OPÇÃO B) ---
            // Garantimos que o fundo é transparente no início
            texto.BackColor = Color.Transparent;
            // Definimos a cor viva inicial (LimeGreen ou Gold)
            texto.ForeColor = corOriginalBoss;
            // ----------------------------------

            int tempoOriginal = minutos * 60; // DICA DE TESTE: int tempoOriginal = 10;
            int tempoAtual = 0;

            System.Windows.Forms.Timer relogio = new System.Windows.Forms.Timer();
            relogio.Interval = 1000;

            botao.Click += (sender, e) =>
            {
                tempoAtual = tempoOriginal;
                botao.Visible = false;
                texto.Visible = true;

                // --- LIMPEZA DE COR (OPÇÃO B) ---
                // Toda vez que clicar, ele "limpa" para a cor original (Lime ou Gold)
                texto.ForeColor = corOriginalBoss;
                // ----------------------------------

                texto.Text = TimeSpan.FromSeconds(tempoAtual).ToString(@"mm\:ss");
                relogio.Start();
            };

            texto.MouseClick += (sender, e) =>
            {
                // Verifica se o clique foi feito com o botão DIREITO do mouse
                if (e.Button == MouseButtons.Right)
                {
                    relogio.Stop(); // Para o motor do tempo
                    texto.Visible = false; // Esconde os números
                    botao.Visible = true;  // Mostra o botão transparente novamente (Boss Vivo)

                    Console.WriteLine($"O cronômetro de {botao.Name} foi resetado pelo usuário.");
                }
            };

            relogio.Tick += (sender, e) =>
            {
                tempoAtual--;
                texto.Text = TimeSpan.FromSeconds(tempoAtual).ToString(@"mm\:ss");

                // --- DESAFIO DA COR (Melhorado) ---
                // Só muda para vermelho se ainda estiver rodando (tempoAtual > 0)
                if (tempoAtual <= 60 && tempoAtual > 0)
                {
                    texto.ForeColor = corDoPerigo;
                }
                // ------------------------------------------

                if (tempoAtual <= 0) // O tempo zerou!
                {
                    relogio.Stop();
                    texto.Visible = false;
                    botao.Visible = true;
                    alarme.Play();

                    // Log no Console formatado (usei WriteLine para organizar)
                    Console.WriteLine($"O boss ligado ao botão {botao.Name} nasceu!");
                }
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lblTempoBossVerde3_Click(object sender, EventArgs e)
        {

        }
    }
}