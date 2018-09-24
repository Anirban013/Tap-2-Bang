using System;

public abstract class RevMobFullscreen {
	public abstract void Show();

    public abstract void Hide();

    public abstract void ShowVideo();

    public abstract void ShowRewardedVideo();

    public virtual void Release() {
        this.Hide();
    }
}

