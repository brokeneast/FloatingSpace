using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleInfoSettingsWindow : MonoBehaviour
{
    [SerializeField] InputField nameInputField = null;
    [SerializeField] InputField descriptionInputField = null;
    [SerializeField] InputField tagInputField = null;
    [SerializeField] Dropdown skinDropdown = null;

    public delegate void EditRoleInfo(RoleInfo info);
    private EditRoleInfo onRoleInfoEdited;
    private RoleCreator roleCreator;
    private RoleInfo roleInfo;

    private void Start()
    {
        nameInputField.onEndEdit.AddListener(SubmitName);
        descriptionInputField.onEndEdit.AddListener(SubmitDescription);
        tagInputField.onEndEdit.AddListener(SubmitTag);
        roleCreator = FindObjectOfType<RoleCreator>();
        SetDropDownOption();
    }

    private void OnEnable()
    {
        skinDropdown.onValueChanged.AddListener(delegate{
            DropdownValueChanged(skinDropdown);
        });


    }

    private void OnDisable()
    {
        skinDropdown.onValueChanged.RemoveListener(delegate{
            DropdownValueChanged(skinDropdown);
        });
    }

    public void Init(RoleInfo roleInfo, EditRoleInfo onRoleInfoEdited)
    {
        this.roleInfo = roleInfo;
        this.onRoleInfoEdited = onRoleInfoEdited;

        Refresh();
    }

    /// <summary>
    /// 關閉並寫回。
    /// </summary>
    public void Close()
    {
        onRoleInfoEdited?.Invoke(roleInfo);
        Destroy(gameObject);
    }

    private void Refresh()
    {
        nameInputField.text = roleInfo.displayName;
        descriptionInputField.text = roleInfo.description;
        tagInputField.text = roleInfo.tag;
    }

    private void SubmitName(string arg0)
    {
        roleInfo.displayName = arg0;
    }

    private void SubmitDescription(string arg0)
    {
        roleInfo.description = arg0;
    }

    private void SubmitTag(string arg0)
    {
        roleInfo.tag = arg0;
    }

    private void SetDropDownOption()
    {
        skinDropdown.ClearOptions();
        List<string> skinNameList = new List<string>();
        for(int i = 0; i < roleCreator.skins.Count; i++)
        {
            skinNameList.Add(roleCreator.skins[i].name);
        }
        skinDropdown.AddOptions(skinNameList);
    }

    private void DropdownValueChanged(Dropdown change)
    {
        Debug.Log(change.value);
        roleInfo.skin = change.value;
    }

}
