using UnityEngine;

public class CollectableDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject model;

    [SerializeField]
    private float spin_rate = 100.0f;

    [SerializeField]
    private float bob_rate = 100.0f;

    [SerializeField]
    private float bob_height = 100.0f;

    void Update()
    {
        model.transform.Rotate(new Vector3(0.0f, spin_rate * Time.deltaTime, 0.0f));
        model.transform.localPosition = new Vector3(0.0f, Mathf.Cos(Time.time * bob_rate) * bob_height, 0.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"I {this.gameObject.name} hit something {other.gameObject.name}");
    }
}
