using UnityEngine;

public interface IInteractable
{
    // void Interact(Interactor interactor);   This is used if we want to tell the object we're interacting with who is doing the interaction, for example if we want to give the player a reference to the object they are interacting with. For now we don't need it so we'll just leave it out. 
    void Interact();

    string GetPromptText();
}

