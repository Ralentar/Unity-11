using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private Color _baseColor = Color.grey;

    private bool _isñCollision = false;
    private float _minDelayTime = 2;
    private float _maxDelayTime = 5;
    private WaitForSeconds _wait;
    private Rigidbody _rigidbody;
    private Material _material;

    public event Action<Cube> Ñollision;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isñCollision)
            return;

        if (collision.gameObject.TryGetComponent(out Platform platform) == false)
            return;

        _isñCollision = true;
        Recolor(UnityEngine.Random.ColorHSV());
        StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return _wait;
        Ñollision?.Invoke(this);
    }

    public void ResetState(Vector3 position, Quaternion rotation)
    {
        _wait = new WaitForSeconds(UnityEngine.Random.Range(_minDelayTime, _maxDelayTime));
        Recolor(_baseColor);

        _isñCollision = false;
        transform.position = position;
        transform.rotation = rotation;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void Recolor(Color color)
    {
        _material.color = color;
    }
}