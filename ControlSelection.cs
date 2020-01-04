using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ControlSelection : MonoBehaviour
{
    public Camera CameraOnGame;
    public LayerMask LayerGolpear;
    public Renderer RendererGuardado;
    public bool GolpeAnterior;
    public int Oportunidades;
    public int Oportunidades2;
    public ArenaDinamica Enemigo;
    public bool SeleccionJugador;
    public List<GameObject> SeleccionesJugador;
    public int EtapaDeLaPartida;
    public GameObject ObjetoPantallaMuerte;
    public Image ImagenMuerte;
    public Button ButtonMuerte1Si;
    public Button ButtonMuerte2No;
    public AudioSource SelectionSource;
    public AudioClip AudioclipSelection;

    public void ActivarImagenMuerte()
    {
        ObjetoPantallaMuerte.SetActive(true);
        ImagenMuerte.DOFade(1, 1);
        ButtonMuerte1Si.GetComponent<Image>().DOFade(1, 1);
        ButtonMuerte2No.GetComponent<Image>().DOFade(1, 1);
    }

    void EsperandoEnemigoPartida()
    {
        if (SeleccionJugador == true)
        {
            if (Oportunidades == 1)
            {
                SeleccionJugador = false;
                Debug.Log("Segunda Vuelta");
                for (int i = 0; i < SeleccionesJugador.Count; i++)
                {
                    SeleccionesJugador[i].GetComponent<Collider>().enabled = true;
                }
                Enemigo.Invoke("SeleccionesAlAzar", 1);
            }
        }
        else
        {
            //Debug.Log("Acabo Seleccion Jugador");
        }
    }

    void Start()
    {
        GolpeAnterior = false;
        SeleccionJugador = true;
        EtapaDeLaPartida = 1;
        Oportunidades = 4;
        Oportunidades2 = 4;
    }


    public void ResetValues()
    {
        SeleccionJugador = true;
        Oportunidades = 4;
        Oportunidades2 = 4;
        GolpeAnterior = false;
        EtapaDeLaPartida = 1;
        SeleccionesJugador = new List<GameObject>();
    }

    public void SegundaEtapaJugador()
    {
        SeleccionesJugador = new List<GameObject>();
        EtapaDeLaPartida = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemigo.LifePlayer >= 0 && Enemigo.LifePlayer <= 1)
        {
            if (EtapaDeLaPartida == 1)
            {
                EsperandoEnemigoPartida();
                RaycastHit hit;
                Ray ray = CameraOnGame.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, LayerGolpear))
                {
                    if (hit.collider != null)
                    {

                        if (hit.collider.GetComponent<OriginalColor>())
                        {
                            RendererGuardado = hit.collider.GetComponent<Renderer>();
                            RendererGuardado.GetComponent<OriginalColor>().IsOn = true;

                            if (SeleccionJugador == true)
                            {
                                if (Input.GetMouseButtonDown(0))
                                {
                                    Oportunidades--;
                                    if (Oportunidades > 0)
                                    {
                                        RendererGuardado.GetComponent<OriginalColor>().IsLoked = true;
                                        RendererGuardado.GetComponent<Collider>().transform.GetChild(0).gameObject.SetActive(true);
                                        RendererGuardado.GetComponent<Collider>().enabled = false;
                                        SeleccionesJugador.Add(hit.collider.gameObject);
                                        SelectionSource.PlayOneShot(AudioclipSelection, 1);
                                    }
                                    else if (Oportunidades <= 1)
                                    {
                                        Debug.Log("Se Acabaron");
                                    }
                                }
                            }
                        }
                        else if (hit.collider.GetComponent<OriginalColor>() == null)
                        {
                            if (hit.collider.name.Contains("Limite"))
                            {
                                if (RendererGuardado != null)
                                {
                                    RendererGuardado.GetComponent<OriginalColor>().IsOn = false;
                                }
                                else
                                {
                                    //Debug.Log("Primer Golpe");
                                }
                            }
                        }
                    }

                }
                else
                {

                    if (RendererGuardado != null)
                    {
                        RendererGuardado.GetComponent<OriginalColor>().IsOn = false;
                    }
                    else
                    {
                        //Debug.Log("No Golpes");
                    }
                }
            }
            else if (EtapaDeLaPartida == 2)
            {
                Seleccionsegunda();
            }
        }
        else if (Enemigo.LifePlayer < 0)
        {
            Debug.Log("Jugador Esta Muerto");
            ActivarImagenMuerte();
        }
    }

    public void Seleccionsegunda()
    {
        RaycastHit hit;
        Ray ray = CameraOnGame.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, LayerGolpear))
        {
            if (hit.collider != null)
            {

                if (hit.collider.GetComponent<OriginalColor>())
                {
                    RendererGuardado = hit.collider.GetComponent<Renderer>();
                    RendererGuardado.GetComponent<OriginalColor>().IsOn = true;

                    if (SeleccionJugador == true)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Oportunidades2--;
                            if (Oportunidades2 > 0)
                            {
                                RendererGuardado.GetComponent<OriginalColor>().IsLoked = true;
                                SeleccionesJugador.Add(hit.collider.gameObject);
                                Enemigo.ComparacionesEnemigo(hit.collider.gameObject);
                            }
                            else if (Oportunidades2 <= 1)
                            {
                                Debug.Log("Se Acabaron");
                            }
                        }
                    }
                }
                else if (hit.collider.GetComponent<OriginalColor>() == null)
                {
                    if (hit.collider.name.Contains("Limite"))
                    {
                        if (RendererGuardado != null)
                        {
                            RendererGuardado.GetComponent<OriginalColor>().IsOn = false;
                        }
                        else
                        {
                            //Debug.Log("Primer Golpe");
                        }
                    }
                }
            }

        }
        else
        {

            if (RendererGuardado != null)
            {
                RendererGuardado.GetComponent<OriginalColor>().IsOn = false;
            }
            else
            {
                //Debug.Log("No Golpes");
            }
        }
    }
}
