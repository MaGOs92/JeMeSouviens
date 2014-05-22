using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerCrepe : MonoBehaviour
{

    #region attributs
    public enum GameState
    {
        queteNoemie,
        preparationPate,
        etalerLeBeurre,
        cuissonCrepe,
        aideDeSkypi
    }

    public static QueteCrepe queteCrepe;

    public Texture2D noemie;
    public Texture2D skypi;
    public Texture2D Tex_dialogue;
    GUIStyle style = new GUIStyle();
    private int brd = Screen.height / 100;

    public static bool boutonValidation = false;
    public static GameState curGameState;
    public static GameState prevGameState;

    public AudioClip musiqueAmbiance;
    public AudioClip miaulementSkypi;
    public AudioClip dragOK;

    public static AudioSource ambiance;
    public static AudioSource miaulement;
    public static AudioSource sonDragOK;

    #endregion attributs

    void Start()
    {
        queteCrepe = GetComponent<QueteCrepe>();

        curGameState = GameState.queteNoemie;

        ambiance = AddAudio(musiqueAmbiance, true, true, 0.5f);
        miaulement = AddAudio(miaulementSkypi, false, false, 0.8f);
        sonDragOK = AddAudio(dragOK, false, false, 0.8f);
        ambiance.Play();

	}
	
	
	#region OnGUI
    void OnGUI()
    {
        print("INGM cur : " + GameManagerCrepe.curGameState + "    prev :  " + GameManagerCrepe.prevGameState + "   " + boutonValidation);

        if (!noemie || !skypi)
        {
            Debug.LogError("Ajouter les textures!");
            return;
        }

        // Affichage de la quete
        if (curGameState == GameState.queteNoemie)
        {
            AfficherDialogue(noemie, queteCrepe.texteQuete());
        }

        // preparation de la pate
        else if (curGameState == GameState.preparationPate)
        {
            // si on a appuye sur le bouton de validation
            if (boutonValidation)
            {
                // si la quete est reussie, on change d'etat
                if (queteCrepe.queteAccomplie())
                {
                    AfficherDialogue(noemie, "Tu as parfaitement réussie la recette!");
                }
                // si la recette a mal été suivie
                else
                {
                    AfficherDialogue(noemie, "Tu t'es trompé dans la recette.");
                }
            }

            else
			{				
				style.fontSize = Screen.height / 36;
				style.alignment = TextAnchor.MiddleCenter + brd;
				style.font = (Font)Resources.Load("Roboto-Regular");

				Rect box = new Rect(Screen.width * 3/4, Screen.height - Screen.height/3, Screen.width/4-brd, Screen.height / 3 - brd);
				GUI.DrawTexture(box, Tex_dialogue, ScaleMode.ScaleAndCrop, true, 0);
				GUI.Box(box, queteCrepe.contenuDuSaladier(), style);
			}

        }

        // etalage du beurre
        else if (curGameState == GameState.etalerLeBeurre)
        {

        }

        // cuisson de la crepe
        else if (curGameState == GameState.cuissonCrepe)
        {

        }

        // affichage de l'aide de skipy
        else if (curGameState == GameState.aideDeSkypi)
        {
            string aide = "";
            switch (prevGameState)
			{
			case GameState.preparationPate:
                    if (!queteCrepe.queteAccomplie())
                    {
                        aide = "Pour mettre des ingrédients dans le saladier, il te suffit de les faire glisser dedans! \nVoici ce qu'il manque :\n";
                        aide += queteCrepe.ingredientManquants();
                    }
                    else
                    {
                        aide = "Tu as mis tous les ingrédients nécessaires!\n clique sur la flèche verte pour passer à l'étape suivante!";
                    }
                    break;
                case GameState.etalerLeBeurre:
                    aide = "Étale le beurre en utilisant ton doigt sur la poële";
                    break;
                case GameState.cuissonCrepe:
                    aide = "Étale la pâte à crêpe en penchant la tablette !";
                    break;
                default:
                    aide = "Je ne sais pas quoi te dire";
                    break;
            }
            AfficherDialogue(skypi, aide);
        }
    }
    #endregion OnGUI


    #region audio

    AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float volume)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = volume;
        return newAudio;
    }

    #endregion


    // Parameters: prev State, curr State
    void ChangeState(GameManagerCrepe.GameState prev, GameManagerCrepe.GameState current)
    {
        GameManagerCrepe.curGameState = current;
        GameManagerCrepe.prevGameState = prev;
    }

    // affiche une boite de dialogue avec comme texture pnj et comme texte txt,
    void AfficherDialogue(Texture2D pnj, string txt)
    {
        style.fontSize = Screen.height / 36;
        style.alignment = TextAnchor.MiddleLeft;
        style.font = (Font)Resources.Load("Roboto-Regular");

        GUI.DrawTexture(new Rect(brd, Screen.height * 2 / 3, Screen.width - brd * 2, Screen.height / 3 - brd), Tex_dialogue, ScaleMode.StretchToFill, true, 0);
        GUI.DrawTexture(new Rect(brd * 2, Screen.height * 2 / 3 + brd, Screen.width / 5, Screen.height / 3 - brd * 3), pnj, ScaleMode.ScaleToFit, true, 0);
        GUI.Box(new Rect(Screen.width * 1 / 4, Screen.height * 7 / 10 + brd * 2, Screen.width - 20, Screen.height / 5 - 10), txt, style);

        //style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = Screen.height / 28;
        GUI.Box(new Rect(Screen.width * 2 / 3, Screen.height * 2 / 3 + brd * 2, Screen.width / 10, Screen.height / 3 - 10), "TOUCHER POUR CONTINUER !", style);
    }

}