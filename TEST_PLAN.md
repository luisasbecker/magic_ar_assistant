# TEST_PLAN

## Testes funcionais

- Validar menu inicial e navegacao entre cenas.
- Validar permissao de camera concedida/negada.
- Validar inicio de nova partida e carregamento da ultima partida.
- Validar vida dos jogadores: +1, -1, +5, -5, reset e undo.
- Validar controle de turno: proxima fase e passar turno.
- Validar log para partida iniciada, vida alterada, fase/turno alterado, carta selecionada, marcador aplicado/removido, undo, save/export.
- Validar resumo final.

## Testes AR

- Imprimir ou exibir os oito alvos PNG.
- Testar deteccao em boa iluminacao, baixa iluminacao, angulo inclinado e reflexos.
- Confirmar que carta detectada cria `CardOverlayView`.
- Confirmar que marcador detectado cria `MarkerOverlayView`.
- Confirmar que overlay acompanha posicao e rotacao de cada alvo.
- Confirmar estado "Rastreamento instavel" quando ARFoundation reportar tracking limitado.
- Confirmar log de tracking perdido/recuperado/instavel.
- Confirmar fallback para modo manual quando AR estiver inutilizavel.

## Testes de persistencia

- Salvar partida nova.
- Fechar/reabrir app e carregar ultima partida.
- Testar arquivo inexistente.
- Corromper JSON salvo e validar mensagem amigavel.
- Exportar log JSON.
- Exportar log TXT.
- Limpar dados locais nas configuracoes.

## Testes de UX

- Verificar legibilidade dos overlays em mesa fisica.
- Verificar tamanho minimo de toque dos botoes.
- Validar contraste de ganho/perda de vida.
- Confirmar que UI 2D nao conflita com toque em objetos AR.
- Validar Safe Area em telas grandes.
- Validar que o app nao cobre a interacao social da mesa com paineis excessivos.

## Testes de compatibilidade

### Android

- Galaxy S25 Ultra.
- Build ARM64.
- IL2CPP.
- ARCore habilitado no XR Plug-in Management.
- Camera permission no Android.

### iOS

- iPhone 16 Pro Max.
- Projeto Xcode gerado.
- ARM64.
- ARKit habilitado no XR Plug-in Management.
- `NSCameraUsageDescription` presente.

## Testes automatizados

Rodar EditMode tests em `MagicARAssistant.Tests`:

- Vida inicial 20/20.
- +1 aumenta vida.
- -1 reduz vida.
- Reset volta para 20.
- Undo restaura valor anterior.
- Proxima fase avanca.
- Passar turno alterna jogador ativo.
- Marcador +1/+1 incrementa contador.
- Remocao nao deixa contador negativo.
- Salvar/carregar JSON preserva estado.
- Log registra alteracao de vida.
- Log registra aplicacao de marcador.

