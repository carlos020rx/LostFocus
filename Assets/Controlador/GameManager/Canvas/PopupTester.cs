using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTester : MonoBehaviour
{
    [Header("Referencias")]
    public PopupMessage message1; // normal
    public PopupMessage message2; // palpitante
    public PopupMessage message3; 
    public PopupMessage message4; 
    public PopupMessage message5; 
    public PopupMessage message6; 
    public PopupMessage message7; 
    public OverlayController overlay; // referencia al fondo oscuro
    public BlinkingPanel blinkingPanel;
    [Header("Control �nico")]
    public bool playSequence; // �nico booleano desde el Inspector
    public bool showMessage4;
    public bool showMessage5;
    public bool blinkAutoTrigger; 
    public bool terminoMensaje1=false;
    public bool showMessage6;
    public bool showMessage7;


    [Header("Tiempos")]
    [SerializeField] private float msg3Lifetime = 3f;   // dura 3s
    [SerializeField] private float afterMsg3Delay = 0.5f; // espera 1s luego de desaparecer
    [SerializeField] private float groupLifetime = 5f;  // cuanto duran juntos msg1 y msg2
    [SerializeField] private float lifetimeSeconds = 5f;
    [SerializeField] private float lifetimeSeconds2 = 3f;// Cuanto duran los otros mensajes
    [SerializeField] private float blinkDurationSeconds = 15f;

    private bool _prevPlay;
    private bool _prev4, _prev5;
    private bool _prev6, _prev7;
    private bool _prevBlinkAuto;
    private Coroutine _sequenceRoutine;
    public AudioSource sonidoAlerta;

    private void Update()
    {

        // Mensaje 4 
        if (showMessage4 && !_prev4)
            message4?.Show(lifetimeSeconds, pulse: false);
        else if (!showMessage4 && _prev4)
            message4?.HideAnimated();
        // Mensaje 5 
        if (showMessage5 && !_prev5)
            message5?.Show(lifetimeSeconds2, pulse: false);
        else if (!showMessage5 && _prev5)
            message5?.HideAnimated();

        // Mensaje 6
        if (showMessage6 && !_prev6)
            message6?.Show(lifetimeSeconds, pulse: false);
        else if (!showMessage6 && _prev6)
            message6?.HideAnimated();

        // Mensaje 7
        if (showMessage7 && !_prev7)
            message7?.Show(lifetimeSeconds, pulse: false);
        else if (!showMessage7 && _prev7)
            message7?.HideAnimated();


        // Flanco de subida: inicia secuencia
        if (playSequence && !_prevPlay)
        {
            // Por si hab�a algo corriendo, cancelo y limpio
            StopSequenceAndHideAll();
            _sequenceRoutine = StartCoroutine(SequenceRoutine());
        }
        // Flanco de bajada: cancelar todo y ocultar inmediatamente
        else if (!playSequence && _prevPlay)
        {
            StopSequenceAndHideAll();
        }

        // Disparo autom�tico del parpadeo independiente (flanco false -> true)
        if (blinkAutoTrigger && !_prevBlinkAuto)
        {
            // Arranca parpadeo por N segundos y se apaga solo
            blinkingPanel?.StartBlink(blinkDurationSeconds);

            // Opcional: autorresetear el booleano para que quede listo para el pr�ximo disparo
            blinkAutoTrigger = false;
        }
        _prevBlinkAuto = blinkAutoTrigger;
        _prevPlay = playSequence;
        _prev4 = showMessage4;
        _prev5 = showMessage5;
    }

    private IEnumerator SequenceRoutine()
    {
        // 1) Muestra Mensaje 3 por 3s (auto-oculta)
        if (message3 != null)
        {
            message3.Show(msg3Lifetime, pulse: false);
            sonidoAlerta.Play();

            // Espera su vida �til + su animaci�n de salida antes del delay extra
            yield return new WaitForSeconds(msg3Lifetime + message3.DisappearDuration);
        }

        // 2) Delay de 1s despu�s de que Mensaje 3 desaparece
        yield return new WaitForSeconds(afterMsg3Delay);

        // Si en medio apagaron el booleano, no sigas
        if (!playSequence) yield break;
        overlay?.Show();
        // 3) Aparecen Mensaje 1 (sin pulso) y Mensaje 2 (con pulso) a la vez
        if (message1 != null) message1.Show(groupLifetime, pulse: false);
        if (message2 != null) message2.Show(groupLifetime, pulse: true);

        // Ambos tienen el mismo lifetime, por lo que se ir�n EXACTAMENTE al mismo tiempo
        yield return new WaitForSeconds(groupLifetime + MaxDisappearDuration(message1, message2));
        overlay?.Hide();

        // playSequence = false;
        _sequenceRoutine = null;
    }

    private float MaxDisappearDuration(PopupMessage a, PopupMessage b)
    {
        float da = a != null ? a.DisappearDuration : 0f;
        float db = b != null ? b.DisappearDuration : 0f;
        return Mathf.Max(da, db);
    }

    private void StopSequenceAndHideAll()
    {
        overlay?.Hide();
        if (_sequenceRoutine != null)
        {
            StopCoroutine(_sequenceRoutine);
            _sequenceRoutine = null;
        }
        // Oculta todo inmediatamente y cancela cualquier animaci�n en curso
        message1?.HideImmediate();
        message2?.HideImmediate();
        message3?.HideImmediate();
        terminoMensaje1 = true;
    }
}