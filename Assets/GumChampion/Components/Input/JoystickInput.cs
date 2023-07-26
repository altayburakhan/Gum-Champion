using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickInput : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    
    private Vector2 _inputVector;// Declare a private variable to hold the input vector
    
    public Vector2 Direction // Property to get the direction of the input
    {
        get
        {
            return _inputVector;// Return the input vector
        }
    }


    public void OnDrag(PointerEventData eventData)// Method to handle the drag event
    {
      
        Vector2 pos;  // Declare a variable to hold the position
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos))// If the position of the pointer event is within the rectangle of the component
        {
            // Normalize the x and y coordinates of the position
            pos.x = (pos.x / GetComponent<RectTransform>().sizeDelta.x);
            pos.y = (pos.y / GetComponent<RectTransform>().sizeDelta.y);
            
            _inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);// Update the input vector based on the position
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;// If the magnitude of the input vector is greater than 1, normalize the input vector
        }
        
        Debug.Log(_inputVector);// Log the input vector for debugging purposes.
    }
    
    public void OnPointerDown(PointerEventData eventData)// Method to handle the pointer down event

    {
        OnDrag(eventData);    // Call the OnDrag method when the pointer is pressed down
    }

    public void OnPointerUp(PointerEventData eventData)// Method to handle the pointer up event
    
    {
        _inputVector = Vector2.zero;    // Reset the input vector to zero when the pointer is released
    }
}