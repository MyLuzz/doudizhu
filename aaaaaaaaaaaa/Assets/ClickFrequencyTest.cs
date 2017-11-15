using UnityEngine;
using System.Collections;
using System;

public class ClickFrequencyTest : MonoBehaviour
{

    private UnityEngine.UI.Text textBox;
    private System.Collections.Generic.List<float> doubleClick;
    private int[] frequency;
    private int doubleClickCount;
    private float fastest;
    private float slowest;

    void Awake()
    {
        if (doubleClick == null)
        {
            doubleClick = new System.Collections.Generic.List<float>(2);
        }
        if (frequency == null)
        { frequency = new int[10]; }
    }
    // Use this for initialization
    void Start()
    {
        textBox = this.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            doubleClick.Add(Time.time);
            var count = doubleClick.Count;
            if (count == 1)
            { textBox.color = Color.red; }
            else if (count == 2)
            {
                var newTime = doubleClick[1] - doubleClick[0];
                if (fastest == 0) { fastest = newTime; }
                if (slowest == 0) { slowest = newTime; }
                if (newTime < fastest) { fastest = newTime; }
                if (slowest < newTime && newTime < 1) { slowest = newTime; }
                UpdateText(newTime);


                textBox.color = Color.black;

                doubleClick.Clear();
            }
        }
    }

    private void UpdateText(float newTime)
    {
        if (0 <= newTime && newTime < 1)
        {
            var index = Mathf.FloorToInt(newTime * 10);
            frequency[index]++;
            doubleClickCount++;
            var builder = new System.Text.StringBuilder();
            builder.AppendFormat("{0}fastest: {1}{0}slowest: {2}",
                Environment.NewLine, fastest, slowest);
            for (int i = 0; i < 10; i++)
            {
                builder.AppendFormat("{0}{1:0.0}s-{2:0.0}s:",
                    Environment.NewLine, ((float)i) / 10, (((float)i)+ 1) / 10);
                var length = (frequency[i] * 100) / doubleClickCount + 1;
                for (int j = 0; j < length; j++)
                {
                    builder.AppendFormat("{0}", ">");
                }
            }
            textBox.text = builder.ToString();
        }
        else
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendFormat("{0}fastest: {1}{0}slowest: {2}",
                Environment.NewLine, fastest, slowest);
            for (int i = 0; i < 10; i++)
            {
                builder.AppendFormat("{0}{1:0.0}s-{2:0.0}s:",
                    Environment.NewLine, ((float)i) / 10, (((float)i) + 1) / 10);
                var length = (frequency[i] * 100) / doubleClickCount + 1;
                for (int j = 0; j < length; j++)
                {
                    builder.AppendFormat("{0}", ">");
                }
            }
            builder.AppendFormat("{0}Last double click is invalid as it takes too long.",
                Environment.NewLine);
            textBox.text = builder.ToString();
        }
    }
}
