using UnityEngine;
using UnityEngine.UI;

public class RoleInfoSettingsWindow : MonoBehaviour
{
    [SerializeField] InputField nameInputField = null;
    [SerializeField] InputField descriptionInputField = null;
    [SerializeField] InputField tagInputField = null;

    public delegate void EditRoleInfo(RoleInfo info);
    private EditRoleInfo onRoleInfoEdited;

    private RoleInfo roleInfo;

    private void Start()
    {
        nameInputField.onEndEdit.AddListener(SubmitName);
        descriptionInputField.onEndEdit.AddListener(SubmitDescription);
        tagInputField.onEndEdit.AddListener(SubmitTag);
    }

    public void Init(RoleInfo roleInfo, EditRoleInfo onRoleInfoEdited)
    {
        this.roleInfo = roleInfo;
        this.onRoleInfoEdited = onRoleInfoEdited;

        Refresh();
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

    /// <summary>
    /// 關閉並寫回。
    /// </summary>
    public void Close()
    {
        onRoleInfoEdited?.Invoke(roleInfo);
        Destroy(gameObject);
    }
}
