
[System.Serializable]
public struct leafSprites
{
    public bugColor color;
    public UnityEngine.Sprite[] sprites;
}

[System.Serializable]
public struct treeAnims
{
    public bugColor color;
    public UnityEngine.RuntimeAnimatorController animator;
}

[System.Serializable]
public struct colorSpritePair
{
    public bugColor color;
    public UnityEngine.Sprite sprite;
}

[System.Serializable]
public struct ResourcePrefab {
    public string name;
    public UnityEngine.GameObject stuff;
}

[System.Serializable]
public struct soundPair
{
    public UnityEngine.AudioClip clip;
    public string name;
}