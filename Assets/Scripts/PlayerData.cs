using Firebase.Firestore;

[FirestoreData]
public class PlayerData
{
    [FirestoreProperty]
    public int PlayerScore { get; set; }

    [FirestoreProperty]
    public Timestamp SessionStart { get; set; }

    [FirestoreProperty]
    public Timestamp SessionEnd { get; set; }
}
