using UnityEngine;

public abstract class RevMobPopup {
    public abstract void Show();

    public abstract void Hide();

    public virtual void Release() {
        this.Hide();
    }
}
