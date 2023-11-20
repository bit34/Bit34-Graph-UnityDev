using System;
using UnityEngine;
using UnityEngine.UI;

public class RectGraphEditPanel : MonoBehaviour
{
    //  MEMBERS
    public int  ColumnCount         { get { return (int)_columnCountSlider.value; } }
    public int  RowCount            { get { return (int)_rowCountSlider.value; } }
    public bool StraightEdgess { get { return _hasStraghtEdgesToggle.isOn; } }
    public bool DiagonalEdgess { get { return _hasDiagonalEdgesToggle.isOn; } }
    //      For Editor
#pragma warning disable 0649
    [SerializeField] private Text   _columnCountLabel;
    [SerializeField] private Slider _columnCountSlider;
    [SerializeField] private Text   _rowCountLabel;
    [SerializeField] private Slider _rowCountSlider;
    [SerializeField] private Toggle _hasStraghtEdgesToggle;
    [SerializeField] private Toggle _hasDiagonalEdgesToggle;
#pragma warning restore 0649
    //      Private
    private bool   _isInitialized;
    private Action _onChangedHandler;

    //  METHODS
    public void Initialize(Action onChangedHandler)
    {
        if (_isInitialized)
        {
            return;
        }

        _isInitialized    = true;
        _onChangedHandler = onChangedHandler;

        _columnCountSlider.onValueChanged.AddListener((float value)=>{ OnChanged(); });
        _rowCountSlider.onValueChanged.AddListener((float value)=>{ OnChanged(); });
        _hasStraghtEdgesToggle.onValueChanged.AddListener((bool value)=>{ OnChanged(); });
        _hasDiagonalEdgesToggle.onValueChanged.AddListener((bool value)=>{ OnChanged(); });
        
        _columnCountLabel.text = "Columns : " + ColumnCount;
        _rowCountLabel.text    = "Rows : "    + RowCount;
    }

    public void Show(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnChanged()
    {
        _columnCountLabel.text = "Columns : " + ColumnCount;
        _rowCountLabel.text    = "Rows : "    + RowCount;
        _onChangedHandler();
    }
}