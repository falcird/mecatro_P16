using System;
using System.Collections;
using Microsoft.SPOT;

namespace Robot_P16.Actions
{
    /// <summary>
    /// Classe utilitaire pour builder plus facilement une action en s�rie
    /// </summary>
    public class ActionEnSerieBuilder
    {
        public Action actionSuivante;
        public ArrayList liste = new ArrayList();
        public String description;

        public ActionEnSerieBuilder(String description)
        {
            this.description = description;
        }
        public ActionEnSerieBuilder() : this(null) { }

        public ActionEnSerieBuilder Add(Action a)
        {
            liste.Add(a);
            return this;
        }

        public ActionEnSerie Build()
        {
            Action[] listeActions = new Action[liste.Count];
            int i = 0;
            foreach (Object o in liste)
                listeActions[i++] = (Action)o;
            return new ActionEnSerie(listeActions, description);
        }
    }

    public class ActionEnSerie : Action
    {

        public readonly Action[] listeActions;

        public ActionEnSerie( Action[] listeActions, String description)
            : base(description)
        {
            this.listeActions = listeActions;
        }

        private int IndexOfFirstUnsucessfulAction()
        {
            int i = 0;
            while (i < listeActions.Length && listeActions[i].Status == ActionStatus.SUCCESS) i++;
            return i;
        }

        public Action FirstUnsucessfulAction()
        {
            int index = IndexOfFirstUnsucessfulAction();
            if (index < listeActions.Length)
                return listeActions[index];
            return null;
        }

        private int IndexOfAction(Action a)
        {
            for (int i = 0; i < listeActions.Length; i++)
            {
                if (listeActions[i].Equals(a))
                    return i;
            }
            return -1;
        }

        private Action GetNextAction(Action a)
        {
            int index = IndexOfAction(a);
            if (index < 0)
                return null;
            index++;
            if (index >= listeActions.Length)
                return null;
            return listeActions[index];
        }

        public override void execute()
        {
            FirstUnsucessfulAction().StatusChangeEvent += this.feedback;
            FirstUnsucessfulAction().execute();
        }



        protected override bool postStatusChangeCheck(ActionStatus oldpreviousStatus)
        {
            switch (this.Status)
            {
                /*case ActionStatus.UNDETERMINED:
                    foreach (Action a in listeActions)
                        a.ResetStatus();
                    break;*/

            }
            return true;
        }

        public override void ResetStatus()
        {
            foreach (Action a in listeActions)
                a.ResetStatus();
            base.ResetStatus();
        }


        /// <summary>
        /// Feedback est apppel� quand une action de la liste d'action change de statut.
        /// </summary>
        /// <param name="a">Action qui a chang� de statut</param>
        private void feedback(Action a)
        {
            int index = IndexOfAction(a);
            if (index >= 0) // L'action est bien dans la liste d'actions
            {
                switch (a.Status) {
                    case ActionStatus.SUCCESS:
                        a.StatusChangeEvent -= this.feedback; // On arr�te d'�co�ter l'action

                        Action actionSuivante = this.GetNextAction(a);
                        Debug.Print("Next action...");
                        if (actionSuivante != null)
                        {
                            Debug.Print("executing...");
                            actionSuivante.StatusChangeEvent += this.feedback;
                            actionSuivante.execute();
                        }
                        else
                        {
                            this.Status = ActionStatus.SUCCESS;
                        }
                        

                        break;

                    case ActionStatus.FAILURE:
                        this.Status = ActionStatus.FAILURE;
                        break;

                    // Attention, ne pas changer le Status � UNDETERMINED en �coutant un changement UNDERTERMINED : boucle infinie
                }
            }
        }

    }
}