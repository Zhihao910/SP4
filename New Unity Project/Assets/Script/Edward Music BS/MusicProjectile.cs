using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProjectile : MonoBehaviour
{
    [SerializeField]
    AudioSource _audioSource;

    //[SerializeField]
    //AudioWrapper frontpeer;
    //[SerializeField]
    //AudioWrapper backpeer;

    //[SerializeField]
    //AudioClip _sample;

    [SerializeField]
    GameObject Projectile;
    [SerializeField]
    GameObject ProjectileDrag;

    private bool spawnBass;
    private bool spawnKick;
    private bool spawnCenter;
    private bool spawnMelody;
    private bool spawnHigh;
    private bool spawnTwo;
    private bool spawnThree;

    // List of beats
    public List<float> bassList = new List<float>();
    public List<float> kickList = new List<float>();
    private List<float> centerList = new List<float>();
    private List<float> melodyList = new List<float>();
    public List<float> highList = new List<float>();

    public bool detected = false;
    private List<float> threeList = new List<float>();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update ()
    {
        if (detected)
            return;

        // BASS
        if (SpawnEffect._spawnBass)
        {
            if (!spawnBass)
            {
                spawnBass = true;
                Instantiate(Projectile, new Vector2(15, 1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
                bassList.Add(_audioSource.time);
            }
            else if (spawnBass)
            {
                Instantiate(ProjectileDrag, new Vector2(15, 1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }
        }
        else
        {
            spawnBass = false;
        }

        // KICK
        if (SpawnEffect._spawnKick)
        {
            if (!spawnKick)
            {
                spawnKick = true;
                Instantiate(Projectile, new Vector2(15, 0), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
                kickList.Add(_audioSource.time);
            }
            else if (spawnKick)
            {
                Instantiate(ProjectileDrag, new Vector2(15, 0), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }
        }
        else
        {
            spawnKick = false;
        }

        // All 3
        if (SpawnEffect._spawnThree)
        {
            if (!spawnThree)
            {
                spawnThree = true;
                Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
                threeList.Add(_audioSource.time);
            }
            else if (spawnThree)
            {
                Instantiate(ProjectileDrag, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
            }
        }
        else
        {
            spawnThree = false;
        }

        // All 3
        //if (SpawnEffect._spawnCenter)
        //{
        //    if (!spawnCenter)
        //    {
        //        spawnCenter = true;
        //        Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //        threeList.Add(_audioSource.time);
        //    }
        //    else if (spawnCenter)
        //    {
        //        Instantiate(ProjectileDrag, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //    }
        //}
        //else
        //{
        //    spawnCenter = false;
        //}

        //if (SpawnEffect._spawnMelody)
        //{
        //    if (!spawnMelody)
        //    {
        //        spawnMelody = true;
        //        Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //        threeList.Add(_audioSource.time);
        //    }
        //    else if (spawnMelody)
        //    {
        //        Instantiate(ProjectileDrag, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //    }
        //}
        //else
        //{
        //    spawnMelody = false;
        //}

        //if (SpawnEffect._spawnHigh)
        //{
        //    if (!spawnHigh)
        //    {
        //        spawnHigh = true;
        //        Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //        threeList.Add(_audioSource.time);
        //    }
        //    else if (spawnHigh)
        //    {
        //        Instantiate(ProjectileDrag, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //    }
        //}
        //else
        //{
        //    spawnHigh = false;
        //}

        //// ACTUALLY ALSO PART OF VOCALS AND INSTRUMENTS
        //if (SpawnEffect._spawnCenter)
        //{
        //    if (!spawnCenter)
        //    {
        //        //Instantiate(Projectile, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

        //        //spawnCenter = true;

        //        centerList.Add(_audioSource.time);
        //    }
        //    else if (spawnCenter)
        //    {
        //        //Instantiate(ProjectileDrag, new Vector2(15, -1), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //    }
        //}
        //else
        //{
        //    spawnCenter = false;
        //}

        //// VOCALS AND INSTRUMENTS
        //if (SpawnEffect._spawnMelody)
        //{
        //    if (!spawnMelody)
        //    {
        //        //Instantiate(Projectile, new Vector2(15, -2), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

        //        //spawnMelody = true;

        //        melodyList.Add(_audioSource.time);
        //    }
        //    else if (spawnMelody)
        //    {
        //        //Instantiate(ProjectileDrag, new Vector2(15, -2), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //    }
        //}
        //else
        //{
        //    spawnMelody = false;
        //}

        //// CHALKBOARD SCREECHING
        //if (SpawnEffect._spawnHigh)
        //{
        //    if (!spawnHigh)
        //    {
        //        //Instantiate(Projectile, new Vector2(15, -3), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));

        //        //spawnHigh = true;

        //        highList.Add(_audioSource.time);
        //    }
        //    else if (spawnHigh)
        //    {
        //        //Instantiate(ProjectileDrag, new Vector2(15, -3), Quaternion.identity).GetComponent<Projectile>().SetDir(new Vector3(-1, 0, 0));
        //    }
        //}
        //else
        //{
        //    spawnHigh = false;
        //}
    }

    public static void Swap()
    {
    }

    //public void Play()
    //{
    //    frontpeer.StartPlaying();
    //}

    //public float TimeNow()
    //{
    //    return frontpeer.TimeNow();
    //}

    public void saveSong()
    {
        if (Saving.LoadingFromFile("MusicData.txt", (List<string> _data) =>
        {
            return !_data.Contains("<FreqName>" + _audioSource.clip.name.ToString());
        }))
        {
            print("saving");

            List<string> saver = new List<string>();

            saver.Add("<beat>");

            saver.Add("<FreqName>" + _audioSource.clip.name.ToString());
            saver.Add("</FreqName>");

            saver.Add("<basscount>");
            saver.Add((bassList.Count - 1).ToString());
            saver.Add("</basscount>");

            saver.Add("<bass>");
            for (int i = 0; i < (bassList.Count - 1); ++i)
            {
                saver.Add("<ba" + i.ToString() + ">");
                saver.Add(bassList[i].ToString());
                saver.Add("<ba/" + i.ToString() + ">");
            }
            saver.Add("</bass>");

            saver.Add("<kickcount>");
            saver.Add((kickList.Count - 1).ToString());
            saver.Add("</kickcount>");

            saver.Add("<kick>");
            for (int i = 0; i < (kickList.Count - 1); ++i)
            {
                saver.Add("<ki" + i.ToString() + ">");
                saver.Add(kickList[i].ToString());
                saver.Add("<ki/" + i.ToString() + ">");
            }
            saver.Add("</kick>");

            saver.Add("<threecount>");
            saver.Add((threeList.Count - 1).ToString());
            saver.Add("</threecount>");

            saver.Add("<three>");
            for (int i = 0; i < (threeList.Count - 1); ++i)
            {
                saver.Add("<tre" + i.ToString() + ">");
                saver.Add(threeList[i].ToString());
                saver.Add("<tre/" + i.ToString() + ">");
            }
            saver.Add("</three>");

            //saver.Add("<centercount>");
            //saver.Add((centerList.Count - 1).ToString());
            //saver.Add("</centercount>");

            //saver.Add("<center>");
            //for (int i = 0; i < (centerList.Count - 1); ++i)
            //{
            //    saver.Add("<ce" + i.ToString() + ">");
            //    saver.Add(centerList[i].ToString());
            //    saver.Add("<ce/" + i.ToString() + ">");
            //}
            //saver.Add("</center>");

            //saver.Add("<melodycount>");
            //saver.Add((melodyList.Count - 1).ToString());
            //saver.Add("</melodycount>");

            //saver.Add("<melody>");
            //for (int i = 0; i < (melodyList.Count - 1); ++i)
            //{
            //    saver.Add("<me" + i.ToString() + ">");
            //    saver.Add(melodyList[i].ToString());
            //    saver.Add("<me/" + i.ToString() + ">");
            //}
            //saver.Add("</melody>");

            //saver.Add("<highcount>");
            //saver.Add((highList.Count - 1).ToString());
            //saver.Add("</highcount>");

            //saver.Add("<high>");
            //for (int i = 0; i < (highList.Count - 1); ++i)
            //{
            //    saver.Add("<hi" + i.ToString() + ">");
            //    saver.Add(highList[i].ToString());
            //    saver.Add("<hi/" + i.ToString() + ">");
            //}
            //saver.Add("</high>");

            saver.Add("</beat>");

            saver.Add(_audioSource.clip.name.ToString() + "FreqEnd\n");

            Saving.SaveToFile("MusicData.txt", saver);
        }
        else
        {
            print("Failed to save!");
        }
    }

    public bool checkSong()
    {
        if (Saving.LoadingFromFile("MusicData.txt", (List<string> _data) =>
        {
            // See if song exists
            if (_data.Contains("<FreqName>" + _audioSource.clip.name.ToString()))
                print("Song Data found, reading now...");
            else
            {
                print("couldnt find song name:" + _audioSource.clip.name.ToString());

                return false;
            }

            List<string> freqData = new List<string>();
            freqData = _data.GetRange(_data.IndexOf("<FreqName>" + _audioSource.clip.name.ToString()), (_data.IndexOf(_audioSource.clip.name.ToString() + "FreqEnd") - _data.IndexOf("<FreqName>" + _audioSource.clip.name.ToString())));

            // FIND BASS COUNT
            List<string> bassSegment = new List<string>();
            bassSegment = freqData.GetRange(freqData.IndexOf("<basscount>"), (freqData.IndexOf("</basscount>") - freqData.IndexOf("<basscount>")));
            bassSegment.Remove("<basscount>");
            string basssegmentString = "";
            foreach (string num in bassSegment)
                basssegmentString += num;

            // FIND BASS
            List<string> freqBass = freqData.GetRange(freqData.IndexOf("<bass>"), (freqData.IndexOf("</bass>") - freqData.IndexOf("<bass>")));
            freqBass.Remove("<bass>");

            for (int i = 0; i < (Convert.ToInt32(basssegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            {
                List<string> bassData = freqBass.GetRange(freqBass.IndexOf("<ba" + i.ToString() + ">"), (freqBass.IndexOf("<ba/" + i.ToString() + ">") - freqBass.IndexOf("<ba" + i.ToString() + ">")));
                bassData.Remove("<ba" + i.ToString() + ">");

                string freqString = "";
                foreach (string num in bassData)
                    freqString += num;

                bassList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }

            // FIND KICK COUNT
            List<string> kickSegment = new List<string>();
            kickSegment = freqData.GetRange(freqData.IndexOf("<kickcount>"), (freqData.IndexOf("</kickcount>") - freqData.IndexOf("<kickcount>")));
            kickSegment.Remove("<kickcount>");
            string kicksegmentString = "";
            foreach (string num in kickSegment)
                kicksegmentString += num;

            // FIND KICK
            List<string> freqkick = freqData.GetRange(freqData.IndexOf("<kick>"), (freqData.IndexOf("</kick>") - freqData.IndexOf("<kick>")));
            freqkick.Remove("<kick>");

            for (int i = 0; i < (Convert.ToInt32(kicksegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            {
                List<string> kickData = freqkick.GetRange(freqkick.IndexOf("<ki" + i.ToString() + ">"), (freqkick.IndexOf("<ki/" + i.ToString() + ">") - freqkick.IndexOf("<ki" + i.ToString() + ">")));
                kickData.Remove("<ki" + i.ToString() + ">");

                string freqString = "";
                foreach (string num in kickData)
                    freqString += num;

                kickList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }

            // FIND THREE COUNT
            List<string> threeSegment = new List<string>();
            threeSegment = freqData.GetRange(freqData.IndexOf("<threecount>"), (freqData.IndexOf("</threecount>") - freqData.IndexOf("<threecount>")));
            threeSegment.Remove("<threecount>");
            string threesegmentString = "";
            foreach (string num in threeSegment)
                threesegmentString += num;

            // FIND THREE
            List<string> freqthree = freqData.GetRange(freqData.IndexOf("<three>"), (freqData.IndexOf("</three>") - freqData.IndexOf("<three>")));
            freqthree.Remove("<three>");

            for (int i = 0; i < (Convert.ToInt32(threesegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            {
                List<string> threeData = freqthree.GetRange(freqthree.IndexOf("<tre" + i.ToString() + ">"), (freqthree.IndexOf("<tre/" + i.ToString() + ">") - freqthree.IndexOf("<tre" + i.ToString() + ">")));
                threeData.Remove("<tre" + i.ToString() + ">");

                string freqString = "";
                foreach (string num in threeData)
                    freqString += num;

                threeList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }

            //// FIND CENTER COUNT
            //List<string> centerSegment = new List<string>();
            //centerSegment = freqData.GetRange(freqData.IndexOf("<centercount>"), (freqData.IndexOf("</centercount>") - freqData.IndexOf("<centercount>")));
            //centerSegment.Remove("<centercount>");
            //string centersegmentString = "";
            //foreach (string num in centerSegment)
            //    centersegmentString += num;

            //// FIND CENTER
            //List<string> freqcenter = freqData.GetRange(freqData.IndexOf("<center>"), (freqData.IndexOf("</center>") - freqData.IndexOf("<center>")));
            //freqcenter.Remove("<center>");

            //for (int i = 0; i < (Convert.ToInt32(centersegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            //{
            //    List<string> centerData = freqcenter.GetRange(freqcenter.IndexOf("<ce" + i.ToString() + ">"), (freqcenter.IndexOf("<ce/" + i.ToString() + ">") - freqcenter.IndexOf("ce" + i.ToString() + ">")));
            //    centerData.Remove("<ce" + i.ToString() + ">");

            //    string freqString = "";
            //    foreach (string num in centerData)
            //        freqString += num;

            //    centerList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            //}

            //// FIND MELODY COUNT
            //List<string> melodySegment = new List<string>();
            //melodySegment = freqData.GetRange(freqData.IndexOf("<melodycount>"), (freqData.IndexOf("</melodycount>") - freqData.IndexOf("<melodycount>")));
            //melodySegment.Remove("<melodycount>");
            //string melodysegmentString = "";
            //foreach (string num in melodySegment)
            //    melodysegmentString += num;

            //// FIND MELODY
            //List<string> freqmelody = freqData.GetRange(freqData.IndexOf("<melody>"), (freqData.IndexOf("</melody>") - freqData.IndexOf("<melody>")));
            //freqmelody.Remove("<melody>");

            //for (int i = 0; i < (Convert.ToInt32(melodysegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            //{
            //    List<string> melodyData = freqmelody.GetRange(freqmelody.IndexOf("<me" + i.ToString() + ">"), (freqmelody.IndexOf("<me/" + i.ToString() + ">") - freqmelody.IndexOf("<me" + i.ToString() + ">")));
            //    melodyData.Remove("<me" + i.ToString() + ">");

            //    string freqString = "";
            //    foreach (string num in melodyData)
            //        freqString += num;

            //    melodyList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            //}

            //// FIND HIGH COUNT
            //List<string> highSegment = new List<string>();
            //highSegment = freqData.GetRange(freqData.IndexOf("<highcount>"), (freqData.IndexOf("</highcount>") - freqData.IndexOf("<highcount>")));
            //highSegment.Remove("<highcount>");
            //string highsegmentString = "";
            //foreach (string num in highSegment)
            //    highsegmentString += num;

            //// FIND HIGH
            //List<string> freqhigh = freqData.GetRange(freqData.IndexOf("<high>"), (freqData.IndexOf("</high>") - freqData.IndexOf("<high>")));
            //freqhigh.Remove("<high>");

            //for (int i = 0; i < (Convert.ToInt32(highsegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            //{
            //    List<string> highData = freqhigh.GetRange(freqhigh.IndexOf("<hi" + i.ToString() + ">"), (freqhigh.IndexOf("<hi/" + i.ToString() + ">") - freqhigh.IndexOf("<hi" + i.ToString() + ">")));
            //    highData.Remove("<hi" + i.ToString() + ">");

            //    string freqString = "";
            //    foreach (string num in highData)
            //        freqString += num;

            //    highList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            //}

            return true;
        }
            ))
        {
            print("Successfully loaded from file.");
            detected = true;
            return true;
        }
        else
        {
            print("Song does not exist or error in reading file. Will detect.");
            detected = false;
            return false;
        }
    }

    public bool ChckSongName(string _name)
    {
        if (Saving.LoadingFromFile("MusicData.txt", (List<string> _data) =>
        {
            // See if song exists
            if (_data.Contains("<FreqName>" + _name))
                print("Song Data found, reading now...");
            else
            {
                print("couldnt find song name:" + _name);

                return false;
            }

            List<string> freqData = new List<string>();
            freqData = _data.GetRange(_data.IndexOf("<FreqName>" + _name), (_data.IndexOf(_name + "FreqEnd") - _data.IndexOf("<FreqName>" + _name)));

            // FIND BASS COUNT
            List<string> bassSegment = new List<string>();
            bassSegment = freqData.GetRange(freqData.IndexOf("<basscount>"), (freqData.IndexOf("</basscount>") - freqData.IndexOf("<basscount>")));
            bassSegment.Remove("<basscount>");
            string basssegmentString = "";
            foreach (string num in bassSegment)
                basssegmentString += num;

            // FIND BASS
            List<string> freqBass = freqData.GetRange(freqData.IndexOf("<bass>"), (freqData.IndexOf("</bass>") - freqData.IndexOf("<bass>")));
            freqBass.Remove("<bass>");

            for (int i = 0; i < (Convert.ToInt32(basssegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            {
                List<string> bassData = freqBass.GetRange(freqBass.IndexOf("<ba" + i.ToString() + ">"), (freqBass.IndexOf("<ba/" + i.ToString() + ">") - freqBass.IndexOf("<ba" + i.ToString() + ">")));
                bassData.Remove("<ba" + i.ToString() + ">");

                string freqString = "";
                foreach (string num in bassData)
                    freqString += num;

                bassList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }

            // FIND KICK COUNT
            List<string> kickSegment = new List<string>();
            kickSegment = freqData.GetRange(freqData.IndexOf("<kickcount>"), (freqData.IndexOf("</kickcount>") - freqData.IndexOf("<kickcount>")));
            kickSegment.Remove("<kickcount>");
            string kicksegmentString = "";
            foreach (string num in kickSegment)
                kicksegmentString += num;

            // FIND KICK
            List<string> freqkick = freqData.GetRange(freqData.IndexOf("<kick>"), (freqData.IndexOf("</kick>") - freqData.IndexOf("<kick>")));
            freqkick.Remove("<kick>");

            for (int i = 0; i < (Convert.ToInt32(kicksegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            {
                List<string> kickData = freqkick.GetRange(freqkick.IndexOf("<ki" + i.ToString() + ">"), (freqkick.IndexOf("<ki/" + i.ToString() + ">") - freqkick.IndexOf("<ki" + i.ToString() + ">")));
                kickData.Remove("<ki" + i.ToString() + ">");

                string freqString = "";
                foreach (string num in kickData)
                    freqString += num;

                kickList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }

            // FIND THREE COUNT
            List<string> threeSegment = new List<string>();
            threeSegment = freqData.GetRange(freqData.IndexOf("<threecount>"), (freqData.IndexOf("</threecount>") - freqData.IndexOf("<threecount>")));
            threeSegment.Remove("<threecount>");
            string threesegmentString = "";
            foreach (string num in threeSegment)
                threesegmentString += num;

            // FIND THREE
            List<string> freqthree = freqData.GetRange(freqData.IndexOf("<three>"), (freqData.IndexOf("</three>") - freqData.IndexOf("<three>")));
            freqthree.Remove("<three>");

            for (int i = 0; i < (Convert.ToInt32(threesegmentString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) - 1); ++i)
            {
                List<string> threeData = freqthree.GetRange(freqthree.IndexOf("<tre" + i.ToString() + ">"), (freqthree.IndexOf("<tre/" + i.ToString() + ">") - freqthree.IndexOf("<tre" + i.ToString() + ">")));
                threeData.Remove("<tre" + i.ToString() + ">");

                string freqString = "";
                foreach (string num in threeData)
                    freqString += num;

                threeList.Add(Convert.ToSingle(freqString, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }

            return true;
        }
            ))
        {
            print("Successfully loaded from file.");
            detected = true;
            return true;
        }
        else
        {
            print("Song does not exist or error in reading file. Will detect.");
            detected = false;
            return false;
        }
    }
}
