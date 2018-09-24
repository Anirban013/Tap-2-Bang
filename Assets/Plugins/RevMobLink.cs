using System;

public abstract class RevMobLink {
	public abstract void Open();

    public abstract void Cancel();

    public virtual void Release() {
        this.Cancel();
    }
}
