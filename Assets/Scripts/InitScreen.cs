using System.Collections;
using TMPro;
using UnityEngine;

public class InitScreen : MonoBehaviour
{
    public TMP_Text textoPantalla;
    public float tiempoEntreLineasBoot = 0.1f;
    private string[] lineasBoot = new string[]
    {
        ">> Inicializando Kernel v1.4.55 ... OK",
        ">> Cargando módulos de seguridad ... OK",
        ">> Sincronizando clúster de energía ... OK",
        ">> Verificando integridad del reactor ... OK",
        ">> Montando particiones del sistema ... OK",
        ">> Preparing UI/OS subsystem ... OK",
        ">> Sistema base listo.",
        ""
    };

    [Header("Configuración Barra")]
    public float tiempoEntrePasos = 0.05f;
    public int pasosTotales = 50;

    private string plantilla =
@"╔═════════════════════════════════════════════════════════════╗
║                                                             ║
║ ██████╗  ███████╗  █████╗    █████╗ ██████╗  ████╗   █████╗ ║
║ ██╔══██╗ ██╔════╝ ██╔══██╗ ██╔════╝   ██╔═╝ █╔╝ ██╗  ██╔══╝ ║
║ ██████╔╝ █████╗   ███████║ ██║        ██║   █║  ██║  █████╗ ║
║ ██╔══██╗ ██╔══╝   ██╔══██║ ██║        ██║   █║  ██║   ╚═██║ ║
║ ██║  ██║ ███████╗ ██║  ██║ ╚██████╗   ██║    ████╔╝  █████║ ║
║ ╚═╝  ╚═╝ ╚══════╝ ╚═╝  ╚═╝  ╚═════╝   ╚═╝     ╚══╝    ╚═══╝ ║
║                                                             ║
║                * * * R E A C T O R   O S   * * *            ║
║                                                             ║
║   [BAR] PCT%                                                ║
║                                                             ║
║     CARGANDO DATOS DEL SISTEMA... POR FAVOR ESPERE...       ║
║                                                             ║
╚═════════════════════════════════════════════════════════════╝";

    private bool sistemaListo = false;
    private bool esperandoEnter = false;

    void Start()
    {
        StartCoroutine(SecuenciaInicio());
    }

    IEnumerator SecuenciaInicio()
    {
        // 1) Mostrar boot línea por línea
        textoPantalla.text = "";
        foreach (string linea in lineasBoot)
        {
            textoPantalla.text += linea + "\n";
            yield return new WaitForSeconds(tiempoEntreLineasBoot);
        }

        // 2) Ejecutar la animación de carga del sistema
        yield return StartCoroutine(CargarSistema());

        // 3) Cuando termina, queda en espera del usuario
        esperandoEnter = true;
    }

    IEnumerator CargarSistema()
    {
        for (int i = 0; i <= pasosTotales; i++)
        {
            string completada = new string('█', i);
            string pendiente = new string('░', pasosTotales - i);
            int porcentaje = Mathf.RoundToInt((i / (float)pasosTotales) * 100);

            string barraFinal = $"[{completada}{pendiente}]";

            string textoFinal = plantilla
                .Replace("[BAR]", barraFinal.PadRight(pasosTotales + 7))
                .Replace("PCT%", $"{porcentaje:000}");

            textoPantalla.text = textoFinal;

            yield return new WaitForSeconds(tiempoEntrePasos);
        }

        // 4) Cambiar mensaje final
        textoPantalla.text = plantilla
            .Replace("[BAR]", "[██████████████████████████████████████████████]")
            .Replace("PCT%", "100")
            .Replace("CARGANDO DATOS DEL SISTEMA... POR FAVOR ESPERE...",
                "        SISTEMA CARGADO — PRESIONE [ENTER]        ");

        sistemaListo = true;
    }

    void Update()
    {
        if (esperandoEnter && Input.GetKeyDown(KeyCode.Return))
        {
            esperandoEnter = false;

            Debug.Log("=== ACCESO AL SISTEMA AUTORIZADO ===");
            Debug.Log("Usuario ingresó a ReactorOS en: " + System.DateTime.Now);

        }
    }
}
