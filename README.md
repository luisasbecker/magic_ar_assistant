# Magic AR Assistant

Protótipo academico funcional em Unity para apoiar partidas fisicas de TCG com Realidade Aumentada. O app usa alvos ficticios para reconhecer cartas e marcadores sobre a mesa, exibe overlays em AR, controla vida, turno/fase, marcadores, log e persistencia local.

Este projeto e nao oficial, nao afiliado a Wizards of the Coast/Hasbro e nao usa arte, nomes, textos ou imagens oficiais de Magic: The Gathering. As cartas e marcadores sao ficticios.

## Objetivo academico

O MVP segue a proposta da Luisa Becker dos Santos: reduzir erros de contagem manual, diminuir carga cognitiva e aumentar confianca no estado da partida sem atrapalhar a interacao social da mesa. Os PDFs do planejamento/XRDD indicam foco em clareza dos overlays, HUD de vida, botoes touch, feedback visual/sonoro/haptico e fallback manual quando o AR falhar.

## Funcionalidades do MVP

- Menu inicial com inicio AR, modo manual, carregar ultima partida, configuracoes e sair.
- Fluxo de permissao de camera com mensagem de privacidade.
- Cena AR com AR Session, XR Origin, AR Camera, ARTrackedImageManager, ARRaycastManager, HUD e gerenciador de estado.
- Image tracking com oito alvos placeholder: quatro cartas ficticias e quatro marcadores ficticios.
- Overlays AR world-space para cartas e marcadores.
- Selecao de carta, aplicacao/remocao de marcadores e destaque visual.
- HUD 2D para vida de Jogador A/Jogador B, undo, log, turno e fase.
- Modo manual sem camera/AR.
- Resumo final e exportacao de log JSON/TXT.
- Persistencia local em `Application.persistentDataPath`.
- Configuracoes locais com PlayerPrefs.
- Testes EditMode para regras principais.

## Stack

- Unity 6.x LTS (`6000.0.38f1` recomendado neste scaffold).
- C#.
- Unity AR Foundation `6.0.5`.
- Google ARCore XR Plug-in `6.0.5`.
- Apple ARKit XR Plug-in `6.0.5`.
- UGUI.
- JSON local via `JsonUtility`.
- Sem backend, login ou dependencia de internet para funcionamento principal.

## Como abrir

1. Abra o Unity Hub.
2. Adicione a pasta deste repositorio: `C:\Users\KABUM\OneDrive - Associacao Antonio Vieira\Documentos\Aplicacao_XR`.
3. Abra com Unity 6.x.
4. Aguarde o Package Manager restaurar os pacotes.
5. No menu do Unity, execute `Magic AR Assistant > Preparar projeto para build`.
6. Abra `Assets/Scenes/MainMenuScene.unity`.

## Cenas

- `Assets/Scenes/MainMenuScene.unity`
- `Assets/Scenes/ARMatchScene.unity`
- `Assets/Scenes/ManualModeScene.unity`
- `Assets/Scenes/SettingsScene.unity`
- `Assets/Scenes/MatchSummaryScene.unity`

Cada cena contem um `SceneAutoInstaller`, que monta UI, AR e managers em runtime.

## Dados ficticios

Os dados iniciais ficam em:

- `Assets/StreamingAssets/Data/cards.json`
- `Assets/StreamingAssets/Data/markers.json`

Para adicionar carta ficticia, inclua um novo objeto em `cards.json` e crie uma imagem PNG com o mesmo `referenceImageName` em:

- `Assets/ReferenceImages`
- `Assets/Resources/ReferenceImages`

Depois rode `Magic AR Assistant > Preparar projeto para build` para deixar as texturas legiveis.

## Image tracking

O app tenta criar uma biblioteca mutavel em runtime usando os PNGs em `Assets/Resources/ReferenceImages`. Os alvos tambem existem em `Assets/ReferenceImages` para impressao/teste fisico.

Alvos de carta:

- `CARD_CREATURE_001`
- `CARD_LAND_001`
- `CARD_SPELL_001`
- `CARD_COMMANDER_001`

Alvos de marcador:

- `MARKER_PLUS_ONE`
- `MARKER_DAMAGE`
- `MARKER_ABILITY`
- `MARKER_LIFE_GAIN`

## Build Android: Galaxy S25 Ultra

1. Instale Android Build Support, SDK/NDK e OpenJDK pelo Unity Hub.
2. Execute `Magic AR Assistant > Preparar projeto para build`.
3. Acesse `Edit > Project Settings > XR Plug-in Management`.
4. Em Android, habilite `ARCore`.
5. Em `Player > Android`:
   - Scripting Backend: `IL2CPP`.
   - Target Architectures: `ARM64`.
   - Minimum API Level: Android 8.0/API 26 ou superior.
   - Camera permission deve aparecer no manifesto pelo uso de ARCore/camera.
6. `File > Build Settings > Android > Switch Platform`.
7. Conecte o Galaxy S25 Ultra com depuracao USB.
8. Use `Build And Run` ou `Magic AR Assistant > Build > Android APK`.

Checklist especifico: validar permissao de camera, inicio da AR Session, deteccao dos oito PNGs impressos, estabilidade dos overlays, HUD, modo manual e persistencia local. Este ambiente nao executou teste real no aparelho.

## Build iOS: iPhone 16 Pro Max

1. Abra o projeto no Unity em macOS com iOS Build Support instalado.
2. Execute `Magic AR Assistant > Preparar projeto para build`.
3. Em `Edit > Project Settings > XR Plug-in Management`, habilite `ARKit` para iOS.
4. Em `Player > iOS`:
   - Scripting Backend: `IL2CPP`.
   - Architecture: `ARM64`.
   - `Requires ARKit`: ligado.
   - `Camera Usage Description`: mensagem de privacidade do app.
5. `File > Build Settings > iOS > Switch Platform`.
6. Gere o projeto Xcode via `Build Settings` ou `Magic AR Assistant > Build > iOS Xcode Project`.
7. No Xcode, selecione equipe de assinatura, iPhone 16 Pro Max e rode no dispositivo.

Este ambiente Windows nao gera build iOS real nem valida no iPhone 16 Pro Max.

Comandos detalhados de build nativo estao em `BUILD_NATIVE_APPS.md`.

## Como rodar testes

1. Abra `Window > General > Test Runner`.
2. Escolha `EditMode`.
3. Rode a assembly `MagicARAssistant.Tests`.

Cenarios cobertos: vida inicial, alteracoes de vida, reset, undo, fase, turno, aplicacao/remocao de marcador, persistencia JSON e log.

## Privacidade

O Magic AR Assistant usa a camera apenas localmente para recursos de Realidade Aumentada. O aplicativo nao salva imagens automaticamente e nao envia dados para servidores. O log da partida fica salvo somente neste dispositivo, salvo exportacao manual.

## Limitacoes conhecidas

Consulte `LIMITATIONS.md`.

## Proximos passos

- Validar em dispositivos reais Galaxy S25 Ultra e iPhone 16 Pro Max.
- Ajustar tamanho fisico dos alvos conforme impressao.
- Substituir UI programatica por prefabs polidos depois de validar o fluxo.
- Adicionar feedback sonoro/haptico refinado.
- Melhorar simulacao XR no Editor para testes sem aparelho.
