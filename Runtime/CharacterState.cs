using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dropecho {
  [Serializable]
  public struct CharacterStateValue {
    readonly float value;

    public CharacterStateValue(float value) => this.value = value;
    public CharacterStateValue(bool value) => this.value = value ? 1 : 0;
    public CharacterStateValue(int value) => this.value = value;

    public static implicit operator float(CharacterStateValue state) => state.value;
    public static implicit operator int(CharacterStateValue state) => (int)state.value;
    public static implicit operator bool(CharacterStateValue state) => state.value != 0;

    public static implicit operator CharacterStateValue(float value) => new CharacterStateValue(value);
    public static implicit operator CharacterStateValue(int value) => new CharacterStateValue(value);
    public static implicit operator CharacterStateValue(bool value) => new CharacterStateValue(value);
  }

  public class CharacterState : MonoBehaviour {
    public readonly Dictionary<int, CharacterStateValue> state = new Dictionary<int, CharacterStateValue>();

    public CharacterStateValue SetValue(string name, CharacterStateValue value) => SetValue(NameToHash(name), value);
    public CharacterStateValue SetValue(int hash, CharacterStateValue value) => state[hash] = value;

    public CharacterStateValue GetValue(string name) => GetValue(NameToHash(name));
    public CharacterStateValue GetValue(int hash) => state.ContainsKey(hash) ? state[hash] : 0;

    public float GetValueFloat(string name) => GetValue(NameToHash(name));
    public float GetValueFloat(int hash) => state.ContainsKey(hash) ? state[hash] : 0;

    public bool GetValueBool(string name) => GetValue(NameToHash(name));
    public bool GetValueBool(int hash) => state.ContainsKey(hash) ? state[hash] : false;

    public int GetValueInt(string name) => GetValue(NameToHash(name));
    public int GetValueInt(int hash) => state.ContainsKey(hash) ? state[hash] : 0;

    /// <summary>Turns a string into an int hash for faster retrieval/storage.</summary>
    /// <param name="name">The string to turn into a hash.</param>
    /// <returns>The generated hash.</returns>
    public static int NameToHash(string name) => Animator.StringToHash(name);
  }
}