using System;
using Core.Services.Updater;
using InputReader;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExternalDeviceInputReader : IEntityInputSource, IDisposable
{
    public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
    public float VerticalDirection => Input.GetAxisRaw("Vertical");
    public bool Jump { get; private set; }
    public bool Attack { get; private set; }

    public ExternalDeviceInputReader()
    {
        ProjectUpdater.Instance.UpdateCalled += OnUpdate;
    }

    public void ResetOnTimeActions()
    {
        Jump = false;
        Attack = false;
    }

    public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

    private void OnUpdate()
    {
        if (Input.GetButtonDown("Jump"))
            Jump = true;

        if (!IsPointerOverUI() && Input.GetButtonDown("Fire1"))
            Attack = true;
    }

    private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
}