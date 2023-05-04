using SmartSearch.Abstractions;
using System;
using System.Collections.Generic;

namespace SmartSearch.LuceneNet.Tests.Mocks
{
    internal static class MockDocuments
    {
        public static IDocumentOperation[] ListAll() => new[]
        {
            DocumentOperation.AddOrUpdate("D42428A9", new Dictionary<string, object>
            {
                { "Id", "D42428A9" },
                { "Name", "Geladeira / Refrigerador Electrolux 454 Litros 2 Portas Frost Free Inverse DB53" },
                { "Description", "Geladeira / Refrigerador Electrolux 454 Litros 2 Portas Frost Free Inverse DB53, com diversas funções para a melhor performance do seu Refrigerador.    Características:  Painel Blue Touch: Diversas funções para a melhor performance do seu Refrigerador.  Prateleiras Fast Adapt: Prateleiras deslizantes para organizar os itens de diferentes tamanhos na porta do refrigerador.  Prateleira retrátil: Otimiza o espaço interno, aumentando as opções de uso.  Drink Express: Resfria garrafas e latas de bebidas rapidamente .  Ice Twister: Gelo a qualquer hora de forma prática e simples.  Gavetão de frutas e legumes: Com divisória, organizando melhor o espaço interno.  Porta-ovos removível: Armazenamento e transporte dos ovos de forma prática e segura.  Bandeja deslizante: Espaço para organizar alimentos especiais que necessitam de maior cuidado.  Bottom Freezer: Tipologia voltada para ergonomia, facilitando o uso do refrigerador no dia a dia." },
                { "Price", 3439.80 },
                { "PromotionalPrice", null },
                { "IsInPromotion", false },
                { "Category", "Eletrodomésticos" },
                { "AddedDate", new DateTime(2018, 1, 1) },
                { "GeolocationName", "Impact Hub Curitiba" },
                { "Geolocation", new LatLng(-25.428079, -49.257083) },
                { "Score", 58 }
            }),
            DocumentOperation.AddOrUpdate("BC64C1F2", new Dictionary<string, object>
            {
                { "Id", "BC64C1F2" },
                { "Name", "TV Led HD 28 Polegadas Philco USB HDMI PH28N91D" },
                { "Description", "TV Led HD 28 Polegadas Philco USB HDMI PH28N91D" },
                { "Price", 983.00 },
                { "PromotionalPrice", 950.00 },
                { "IsInPromotion", true },
                { "Category", "Eletrônicos" },
                { "AddedDate", new DateTime(2018, 2, 1) },
                { "GeolocationName", "Parque Barigui" },
                { "Geolocation", new LatLng(-25.418865, -49.305416) },
                { "Score", 36 }
            }),
            DocumentOperation.AddOrUpdate("6EFEEED7", new Dictionary<string, object>
            {
                { "Id", "6EFEEED7" },
                { "Name", "Ventilador Britânia de Mesa Ventus 3 Velocidades" },
                { "Description", "Este Ventilador da Britânia oferece um melhor desempenho, segurança e durabilidade e melhora o direcionamento do fluxo de ar!" },
                { "Price", 107.20 },
                { "PromotionalPrice", null },
                { "IsInPromotion", false },
                { "Category", "Portáteis" },
                { "AddedDate", new DateTime(2018, 1, 16) },
                { "GeolocationName", "Ouro Branco MG" },
                { "Geolocation", new LatLng(-20.509879, -43.712664) },
                { "Score", 12 }
            }),
            DocumentOperation.AddOrUpdate("48BDDFA8", new Dictionary<string, object>
            {
                { "Id", "48BDDFA8" },
                { "Name", "Multifuncional HP Deskjet WI-FI INK Advantage 3776 J9V88AAK4" },
                { "Description", "Impressora multifuncional HP DeskJet Ink Advantage 3776  Economize espaço e dinheiro e imprima sem fio, com a menor impressora multifuncional do mundo.  Obtenha cores a baixo custo e você tem toda a potência que precisa com um estilo incrível e compacto.  Imprima, digitalize e copie, a partir de praticamente qualquer lugar, com esta poderosa impressora multifunções portátil.  A pequena e potente impressora multifuncional:  Economize espaço e tenha todo o poder de que você precisa com a menor impressora multifuncional do mundo.  A HP Scroll Scan ajuda a lidar facilmente com a maioria dos trabalhos de digitalização de uma variedade de papéis.  Em qualquer divisão ou em qualquer lugar, esta impressora multifuncional extremamente compacta foi concebida para se adaptar e caber onde seja necessária.  Exiba o seu estilo com um design elegante e uma variedade de cores deslumbrantes.  Imprima rapidamente a partir do seu dispositivo móvel:  A forma mais fácil de imprimir documentos, fotografias e muito mais a partir dos seus dispositivos Apple, Android e Windows.  Conecte seu smartphone ou tablet diretamente à sua impressora, e imprima com facilidade, sem uma rede.  Conecte-se rapidamente e comece a imprimir com rapidez graças à fácil configuração a partir do seu smartphone ou tablet.  Digitalize qualquer objeto em viagem com a aplicação móvel HP All-in-One Printer Remote para o seu smartphone ou tablet.  Adapta-se ao seu orçamento e adapta-se às suas necessidades:  Imprima, digitalize e copie sem fios aquilo de que necessita de forma rápida e fácil com uma impressora multifuncional acessível.  Conte com uma impressão de qualidade, com cartuchos de tinta HP originais.  Economize energia e tenha os resultados com custos anuais de electricidade de menos de 1,50 por ano.  Recicle facilmente consumíveis HP originais de forma gratuita através do programa HP Planet Partners." },
                { "Price", 433.30 },
                { "PromotionalPrice", null },
                { "IsInPromotion", false },
                { "Category", "Informática" },
                { "AddedDate", new DateTime(2018, 3, 5) },
                { "GeolocationName", "Geneva Switzerland" },
                { "Geolocation", new LatLng(46.204391, 6.143158) },
                { "Score", 23 }
            }),
            DocumentOperation.AddOrUpdate("FC940BFB", new Dictionary<string, object>
            {
                { "Id", "FC940BFB" },
                { "Name", "iPhone X A1901 - 5.8 Polegadas - Single-Sim - 256GB - 4G LTE - Cinza-Espacial" },
                { "Description", "**O Apple iPhone X é um smartphone iOS com características inovadoras que o tornam uma excelente opção para qualquer tipo de utilização. A tela de 5.8 polegadas coloca esse Apple no topo de sua categoria.**    **iPhone X**    Nossa visão sempre foi criar um iPhone que fosse totalmente tela. Tão envolvente que fizesse o aparelho desaparecer na experiência. E tão inteligente que respondesse ao seu toque, voz ou mesmo olhar. O iPhone X é esta visão transformada em realidade. Diga alô para o futuro        **Design e tela**    A tela é tudo..    **Tela Super Retina**    No iPhone X, o aparelho é a própria tela. A inovadora tela Super Retina de 5,8 polegadas foi criada para caber na mão e encher os olhos1.         **Tecnologias inovadoras**    Novas técnicas e tecnologias permitem que a tela acompanhe com precisão as curvas do design, chegando até seus cantos arredondados.         **OLED criado para o iPhone X**    Com cores precisas e deslumbrantes, pretos verdadeiros, brilho elevado e proporção de contraste 1.000.000:1, esta é a primeira tela de OLED que atende o nível de exigência do iPhone.         **Câmera TrueDepth**    Um espaço minúsculo abriga algumas das nossas tecnologias mais sofisticadas, como as câmeras e os sensores que possibilitam o Face ID.         **Novo design**    O iPhone X é construído com o vidro mais resistente já usado em um smartphone — na frente e atrás. Ele tem moldura de aço inoxidável de qualidade cirúrgica, recarga sem fio e toda a estrutura é protegida contra água e poeira.         **Gestos intuitivos**    Gestos simples tornam a navegação natural e intuitiva. Em vez de apertar um botão, é só deslizar o dedo para acessar a tela de Início.         **Face ID**    Reconhecimento facial revolucionário.         **Autenticação segura**    Seu rosto agora é sua senha. O Face ID é a nova forma segura e pessoal de desbloquear e autenticar.         **Mapeamento facial**    O Face ID usa a câmera TrueDepth e é muito simples de configurar. Ele projeta e analisa mais de 30 mil pontos invisíveis para criar um mapa de profundidade preciso do seu rosto.         **Câmera TrueDepth**    A câmera da frente, mais à frente do que nunca.         **Selfies com modo Retrato**    Tire selfies mais bonitas com primeiros planos nítidos e fundos artisticamente desfocados.         **Iluminação de Retrato**    O novo recurso do modo Retrato cria efeitos de iluminação profissional.         **Animoji**    A câmera TrueDepth analisa mais de 50 movimentos musculares diferentes para refletir suas expressões nos 12 Animoji. Revele o panda, gato ou robô que existe em você.         **Câmera dupla de 12 MP**    A arte de tirar fotos lindas com facilidade.         **Câmeras aprimoradas**    Sensor de 12 MP maior e mais rápido. Novo filtro de cores. Pixels mais fiéis. E uma nova câmera teleobjetiva com estabilização óptica de imagem.         **Iluminação de Retrato**    As câmeras com sensor de profundidade e o mapeamento facial preciso possibilitam criar efeitos de iluminação com qualidade de estúdio.         **Dupla estabilização**    As duas câmeras traseiras contam com estabilização óptica de imagem e lentes rápidas para criar fotos e vídeos lindos, mesmo com pouca luz.         **Zoom óptico**    As câmeras com lentes teleobjetiva e grande-angular oferecem zoom óptico, além de zoom digital até 10x para fotos e 6x para vídeos.         **A11 Bionic**    Inteligência sobre-humana.         **Processador neural**    Apresentamos o chip A11 Bionic, o mais inteligente e poderoso em um smartphone. Ele tem um processador neural capaz de realizar até 600 bilhões de operações por segundo.         **CPU mais rápida**    Todos os núcleos da novíssima CPU estão mais rápidos que no A10 Fusion. A velocidade aumentou até 70% nos quatro núcleos de eficiência e até 25% nos dois de desempenho.         **Reconhecimento que se adapta**    O aprendizado de máquina permite que o Face ID se adapte a mudanças na sua aparência ao longo do tempo.         **Consumo inteligente**    Graças ao controlador de desempenho de segunda geração e ao design único da bateria, o iPhone X oferece até duas horas a mais de uso entre recargas do que o iPhone 75.         **GPU criada pela Apple**    A nova GPU de três núcleos criada pela Apple é até 30% mais rápida que no A10 Fusion.         **Realidade aumentada**    O A11 Bionic possibilita jogos e apps com experiências incríveis em realidade aumentada.         **Recarga sem fio**    Recarga feita para um mundo sem fios.         **Recarga sem fio**    Como não precisa de cabos para recarregar, o iPhone X foi realmente desenvolvido para um futuro sem fios.         **AirPower**    Disponível em 2018    Apresentamos o AirPower. Coloque o iPhone, o Apple Watch e os AirPods em qualquer parte da superfície dele para recarregar sem fio6         **iOS 11**    Um iOS à altura de um iPhone tão revolucionário.    .    **Criado para o iPhone X**    Um telefone que é todo tela precisa de um sistema operacional repensado com novos recursos e gestos.         **Novo no iOS 11**    Envie um Animoji no Mensagens, faça da Siri sua DJ particular ou descubra músicas com seus amigos no Apple Music7.         **Realidade aumentada**         Entre de cabeça em jogos e apps e viva experiências inacreditáveis na maior plataforma do mundo para realidade aumentada." },
                { "Price", 7299.90 },
                { "PromotionalPrice", null },
                { "IsInPromotion", false },
                { "Category", "Smartphones" },
                { "AddedDate", new DateTime(2018, 9, 10) },
                { "GeolocationName", "Lausanne Switzerland" },
                { "Geolocation", new LatLng(46.519653, 6.632273) },
                { "Score", 59 }
            }),
            DocumentOperation.AddOrUpdate("222B6FC6", new Dictionary<string, object>
            {
                { "Id", "222B6FC6" },
                { "Name", "Tabua de Passar Melissa 2 Portas - Nicioli" },
                { "Description", "Produto 100% MDP  Puxadores Polipropileno  2 Portas  Acabamento Ultra Violeta  Madeira 100% MDF" },
                { "Price", 169.70 },
                { "PromotionalPrice", null },
                { "IsInPromotion", false },
                { "Category", "Móveis" },
                { "AddedDate", new DateTime(2018, 10, 1) },
                { "GeolocationName", "San Francisco US" },
                { "Geolocation", new LatLng(37.774929, -122.419418) },
                { "Score", 84 }
            })
        };
    }
}