using ProjectUnicorn.InteractionSystem;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

/**
 * File: ConversationManager.cs
 * Description: Connects the UI elements and conversation with the player's interaction.
 */

namespace ProjectUnicorn.DialogSystem
{
    public class ConversationManager : MonoBehaviour, IInteractable
    {
        [SerializeField] private Conversation _conversation;

        [SerializeField] private GameObject _conversationUI;
        [SerializeField] private Image _conversationImage;
        [SerializeField] private TextMeshProUGUI _conversationSpeaker;
        [SerializeField] private TextMeshProUGUI _conversationSpeechText;

        private int _conversationCount = 0;

        private void Start()
        {
            ResetUI();
        }

        public void Interact()
        {
            ShowUI();
            Debug.Log("Activated");

            // Increment the counter by one to progress the conversation
            if (_conversationCount < _conversation.Dialog.Length)
            {
                UpdateUI();
                _conversationCount++;
                Debug.Log("displayed");
            }
            else
            {
                _conversationCount = 0;
                ResetUI();
            }
        }

        public void Interact(Item.ID item) {
        }

        private void ShowUI()
        {
            _conversationUI.SetActive(true);
        }

        private void UpdateUI()
        {
            _conversationImage.sprite = _conversation.Dialog[_conversationCount].CharacterImage;
            _conversationSpeaker.text = _conversation.Dialog[_conversationCount].CharacterName;
            _conversationSpeechText.text = _conversation.Dialog[_conversationCount].CharacterMessage;
        }

        private void ResetUI()
        {
            _conversationUI.SetActive(false);
            _conversationImage.sprite = null;
            _conversationSpeaker.text = null;
            _conversationSpeechText.text = null;
        }
    }
}
