﻿using Newtonsoft.Json.Linq;
using Raylib_cs;
using VTTiny.Components.Data;
using VTTiny.Editor;
using VTTiny.Scenery;

namespace VTTiny.Components
{
    public class SimpleCharacterAnimatorComponent : Component
    {
        /// <summary>
        /// The state of the character
        /// </summary>
        public enum State
        {
            Idle,
            Blinking,
            Speaking
        }

        /// <summary>
        /// The frequency of the blinking
        /// </summary>
        public float BlinkEvery { get; set; } = 1.5f;

        /// <summary>
        /// The length of each blink.
        /// </summary>
        public float BlinkLength { get; set; } = 0.1f;

        /// <summary>
        /// Is the character speaking?
        /// </summary>
        public bool IsSpeaking { get; set; } = false;

        private Texture2D _idle;
        private Texture2D _blinking;
        private Texture2D _speaking;

        private StageTimer _blinkTimer;
        private StageTimer _blinkStartTimer;

        private bool _isBlinking;

        private TextureRendererComponent _renderer;

        /// <summary>
        /// Set a texture for a specific state
        /// </summary>
        /// <param name="texture">Handle to the texture.</param>
        /// <param name="state">The state.</param>
        public void SetTextureForState(Texture2D texture, State state)
        {
            switch (state)
            {
                case State.Idle:
                    _idle = texture;
                    break;
                case State.Blinking:
                    _blinking = texture;
                    break;
                case State.Speaking:
                    _speaking = texture;
                    break;
            }
        }

        public override void Start()
        {
            _blinkTimer = new StageTimer(Parent.OwnerStage);
            _blinkStartTimer = new StageTimer(Parent.OwnerStage);

            _renderer = GetComponent<TextureRendererComponent>();
        }

        public override void Update()
        {
            if (_blinkTimer.TimeElapsed >= BlinkEvery &&
                !_isBlinking)
            {
                _isBlinking = true;
                _blinkStartTimer.SetNow();
            }

            if (_isBlinking &&
                _blinkStartTimer.TimeElapsed >= BlinkLength)
            {
                _isBlinking = false;
                _blinkTimer.SetNow();
            }

            if (IsSpeaking && !_isBlinking)
                _renderer.Texture = _speaking;

            else
                _renderer.Texture = _isBlinking ? _blinking : _idle;
        }

        public override void Destroy()
        {
            Raylib.UnloadTexture(_idle);
            Raylib.UnloadTexture(_blinking);
            Raylib.UnloadTexture(_speaking);
        }

        internal override void InheritParametersFromConfig(JObject parameters)
        {
            var config = JsonObjectToConfig<SimpleCharacterAnimatorConfig>(parameters);
            config.LoadStates(this);

            BlinkEvery = config.BlinkEvery;
            BlinkLength = config.BlinkLength;
        }

        internal override void RenderEditorGUI()
        {
            BlinkEvery = EditorGUI.DragFloat("Blink every", BlinkEvery);
            BlinkLength = EditorGUI.DragFloat("Blink length", BlinkLength);

            if (EditorGUI.DragAndDropTextureButton("Idle", _idle, out Texture2D newIdle))
            {
                Raylib.UnloadTexture(_idle);
                _idle = newIdle;
            }

            if (EditorGUI.DragAndDropTextureButton("Blinking", _blinking, out Texture2D newBlinking))
            {
                Raylib.UnloadTexture(_blinking);
                _blinking = newBlinking;
            }

            if (EditorGUI.DragAndDropTextureButton("Speaking", _speaking, out Texture2D newSpeaking))
            {
                Raylib.UnloadTexture(_speaking);
                _speaking = newSpeaking;
            }
        }
    }
}
