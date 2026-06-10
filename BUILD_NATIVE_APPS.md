# Build dos aplicativos nativos

Este repositório já contém scripts de build para gerar:

- APK Android para Samsung Galaxy S25 Ultra.
- Projeto Xcode iOS para iPhone 16 Pro Max.

## Pré-requisitos

### Android / Samsung

- Unity 6.x com Android Build Support.
- Android SDK/NDK/OpenJDK instalados pelo Unity Hub.
- AR Foundation, ARCore XR Plug-in e XR Plug-in Management restaurados pelo Package Manager.

### iOS / iPhone

- macOS.
- Unity 6.x com iOS Build Support.
- Xcode instalado.
- Conta Apple Developer ou assinatura local válida.
- ARKit XR Plug-in habilitado.

## Gerar APK Android pela interface do Unity

1. Abra o projeto no Unity.
2. Execute `Magic AR Assistant > Preparar projeto para build`.
3. Execute `Magic AR Assistant > Build > Android APK`.
4. O arquivo esperado é `Builds/Android/MagicARAssistant.apk`.

## Gerar APK Android por linha de comando

Exemplo no Windows:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.0.38f1\Editor\Unity.exe" `
  -batchmode `
  -quit `
  -projectPath "C:\Users\KABUM\OneDrive - Associacao Antonio Vieira\Documentos\Aplicacao_XR" `
  -executeMethod MagicARAssistant.Editor.MagicARAssistantBuildCommand.BuildAndroidApk `
  -outputPath "Builds/Android/MagicARAssistant.apk"
```

## Gerar projeto Xcode iOS pela interface do Unity

1. Abra o projeto no Unity em macOS.
2. Execute `Magic AR Assistant > Preparar projeto para build`.
3. Execute `Magic AR Assistant > Build > iOS Xcode Project`.
4. Abra `Builds/iOS/MagicARAssistantXcode` no Xcode.
5. Configure assinatura e rode no iPhone 16 Pro Max.

## Gerar projeto Xcode por linha de comando

```bash
/Applications/Unity/Hub/Editor/6000.0.38f1/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -quit \
  -projectPath "/caminho/para/Aplicacao_XR" \
  -executeMethod MagicARAssistant.Editor.MagicARAssistantBuildCommand.BuildIosXcodeProject \
  -outputPath "Builds/iOS/MagicARAssistantXcode"
```

## Estado deste ambiente

Neste Windows atual não foram encontrados Unity, Android SDK, Gradle ou ADB. Por isso, não foi possível gerar nem instalar o APK aqui. Build iOS também não é possível no Windows; precisa de macOS + Xcode.

