using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    void Attach(IObserver observer);

    void Detach(IObserver observer);

    void Notify();
}
