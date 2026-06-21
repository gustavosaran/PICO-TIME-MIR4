# Mir4 Tactical Boss Timer ⏳ 🎯

Aplicativo Desktop de alta performance desenvolvido em **C# (.NET)** com **Windows Forms**, projetado para gerenciar e rastrear simultaneamente o tempo de renascimento (*respawn*) de múltiplos chefes dentro do jogo MMORPG Mir4 (especificamente para o mapa do Pico Secreto). 

O projeto foi construído do zero com foco em **arquitetura de código limpo (DRY)**, **programação orientada a eventos** e **otimização de UI/UX** para ferramentas de suporte a jogos, servindo como um excelente demonstrativo de habilidades em desenvolvimento desktop e engenharia de software para quem está analisando o perfil.

---

---

## 🚀 Principais Funcionalidades e Diferenciais

* **Fábrica Automática de Cronômetros (Padrão de Fábrica Dinâmica):** Em vez de duplicar código estático para cada chefe no mapa, a aplicação utiliza um método centralizado (`ConfigurarBoss`) que injeta dinamicamente a lógica matemática e os manipuladores de eventos em tempo de execução. Isso reduz o acoplamento e facilita a escalabilidade do software.
* **Interface Imersiva estilo HUD (Heads-Up Display):** Os cronômetros utilizam transparência real (`Alpha Color`) e fontes monoespaçadas de alto contraste (`Consolas`), garantindo que os números flutuem perfeitamente sobre o mapa sem "tremer" a cada segundo, imitando as interfaces nativas de jogos profissionais.
* **Alerta Visual Dinâmico:** Implementação de regras condicionais de tempo que alteram dinamicamente a propriedade visual dos componentes (o texto adquire coloração vermelha de alerta nos últimos 60 segundos de contagem).
* **Mecanismo de Reset Seguro (UX):** Integração do evento `MouseClick` mapeando o botão direito do mouse para cancelamento ou reinicialização rápida da contagem, evitando acionamentos acidentais por clique esquerdo no meio da ação.
* **Tratamento Avançado de Foco do Sistema Operacional:** Blindagem total contra artefatos visuais do Windows através da manipulação do ciclo de vida do formulário (`this.Deactivate`). Isso elimina o "retângulo de foco preto" que o sistema operacional tenta desenhar ao alternar para um segundo monitor ou jogo em tela cheia.
* **Áudio Síncrono:** Integração com a API `SoundPlayer` para disparo de alertas sonoros no momento exato do término de cada contagem.
* **Arquitetura Standalone & Segura:** O software opera em uma camada isolada de memória do Windows, agindo 100% livre de interações ou leituras na memória de outros aplicativos, garantindo total conformidade de segurança.

---

## 🛠️ Competências Técnicas Demonstradas

* **Linguagem de Programação:** C#
* **Framework de UI:** Windows Forms (.NET)
* **Princípios de Software:** DRY (Don't Repeat Yourself), Clean Code e Encapsulamento.
* **Paradigma de Eventos:** Expressões Lambda (`=>`), Funções Anônimas, Subscrição e Manipulação de Eventos Assíncronos (`Click`, `Tick`, `MouseClick`, `Deactivate`).
* **Manipulação de Estados de Interface:** Controle dinâmico de visibilidade (`Visible`), focos de controle (`TabStop`, `ActiveControl`) e formatação avançada de tempo através da estrutura `TimeSpan`.

---

## 📦 Estrutura do Código Principal

Abaixo está o trecho principal que demonstra a automação da lógica de gerenciamento usando conceitos modernos de C#:

```csharp
private void ConfigurarBoss(Button botao, Label texto, int minutos, Color corOriginalBoss)
{
    // Blindagem de interface e tratamento de foco do Windows
    botao.TabStop = false; 
    botao.FlatStyle = FlatStyle.Flat; 
    botao.FlatAppearance.BorderSize = 0; 
    
    texto.BackColor = Color.Transparent; 
    texto.ForeColor = corOriginalBoss; 

    int tempoOriginal = minutos * 60; 
    int tempoAtual = 0;

    System.Windows.Forms.Timer relogio = new System.Windows.Forms.Timer();
    relogio.Interval = 1000;

    // Disparo do Cronômetro (Clique Esquerdo)
    botao.Click += (sender, e) => {
        tempoAtual = tempoOriginal;
        botao.Visible = false;
        texto.Visible = true;
        texto.ForeColor = corOriginalBoss; 
        texto.Text = TimeSpan.FromSeconds(tempoAtual).ToString(@"mm\:ss");
        relogio.Start();
    };

    // Reset Prático e Seguro (Clique Direito)
    texto.MouseClick += (sender, e) => {
        if (e.Button == MouseButtons.Right) {
            relogio.Stop(); 
            texto.Visible = false; 
            botao.Visible = true;  
        }
    };

    // Atualização em Tempo Real (Tick do Timer)
    relogio.Tick += (sender, e) => {
        tempoAtual--;
        texto.Text = TimeSpan.FromSeconds(tempoAtual).ToString(@"mm\:ss");

        if (tempoAtual <= 60 && tempoAtual > 0) {
            texto.ForeColor = Color.Red; // Feedback visual de perigo/atenção
        }

        if (tempoAtual <= 0) {
            relogio.Stop();
            texto.Visible = false;
            botao.Visible = true;
            alarme.Play(); // Alarme sonoro integrado
        }
    };
}
