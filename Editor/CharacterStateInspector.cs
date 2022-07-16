using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Dropecho {
  [CustomEditor(typeof(CharacterState))]
  public class CharacterStateInspector : Editor {
    CharacterState state;

    void OnEnable() {
      state = target as CharacterState;
    }

    public override VisualElement CreateInspectorGUI() {
      // Create a new VisualElement to be the root of our inspector UI
      VisualElement myInspector = new VisualElement();

      foreach (var (key, value) in state.state) {
        myInspector.Add(createItem(key.ToString(), value));
      }

      return myInspector;
    }

    private VisualElement createItem(string label, float value) {
      var item = new VisualElement() {
        style = {
            display = DisplayStyle.Flex,
            flexGrow = 1,
              flexDirection = FlexDirection.Row
          }
      };
      item.Add(new Label(label) {
        style = {
            flexGrow = 0.25f,
          }
      });
      item.Add(new FloatField() {
        value = value,
        style = {
            flexGrow = 1,
          }
      });

      return item;
    }
  }
}