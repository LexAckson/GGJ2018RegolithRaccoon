
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
    public UnityEngine.Animator animator;
}

[System.Serializable]
public struct ResourcePrefab {
    public string name;
    public UnityEngine.GameObject stuff;
}