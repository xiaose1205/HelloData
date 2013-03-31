<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/maste/MainPage.Master" AutoEventWireup="true"
    CodeBehind="UI.aspx.cs" Inherits="HelloData.Web.Test.Admin.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../Style/bootstrap-ie6.css" rel="stylesheet" />
    <link href="../Style/ie.css" rel="stylesheet" />
    <script src="../js/bootstrap-ie.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <button class="btn btn-primary" type="submit">
        测试
    </button>
    <button class="btn " type="submit">
        测试
    </button>

    <input id="Text1" type="text" /><br />
    <input id="File1" type="file" /><br />
    <input id="Password1" type="password" /><br />
    <input id="Checkbox1" type="checkbox" /><br />
    <input id="Radio1" checked="checked" name="R1" type="radio" value="V1" /><br />
    <input id="Hidden1" type="button" class="btn btn-primary" value="dasd"><br />
    <textarea id="TextArea1" cols="20" name="S1" rows="2"></textarea><table style="width: 100%;">
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <img alt="" src="" /><br />
    <select id="Select1" name="D1">
        <option></option>
    </select><hr />
    <a class="btn btn-success" href="#"><i class="icon-zoom-in icon-white"></i>View
    </a><a class="btn btn-info" href="#"><i class="icon-edit icon-white"></i>Edit </a>
    <a class="btn btn-danger" href="#"><i class="icon-trash icon-white"></i>Delete </a>
    <p class="btn-group">
        <button class="btn">
            Left</button>
        <button class="btn">
            Middle</button>
        <button class="btn">
            Right</button>
    </p>
    <p>
        <button class="btn btn-small">
            <i class="icon-star"></i>Icon button</button>
        <button class="btn btn-small btn-primary">
            Small button</button>
        <button class="btn btn-small btn-danger">
            Small button</button>
    </p>
    <p>
        <button class="btn btn-small btn-warning">
            Small button</button>
        <button class="btn btn-small btn-success">
            Small button</button>
        <button class="btn btn-small btn-info">
            Small button</button>
    </p>
    <p>
        <button class="btn btn-small btn-inverse">
            Small button</button>
        <button class="btn btn-large btn-primary btn-round">
            Round button</button>
        <button class="btn btn-large btn-round">
            <i class="icon-ok"></i>
        </button>
        <button class="btn btn-primary">
            <i class="icon-edit icon-white"></i>
        </button>
    </p>
    <p>
        <button class="btn btn-mini">
            Mini button</button>
        <button class="btn btn-mini btn-primary">
            Mini button</button>
        <button class="btn btn-mini btn-danger">
            Mini button</button>
        <button class="btn btn-mini btn-warning">
            Mini button</button>
    </p>
    <p>
        <button class="btn btn-mini btn-info">
            Mini button</button>
        <button class="btn btn-mini btn-success">
            Mini button</button>
        <button class="btn btn-mini btn-inverse">
            Mini button</button>
    </p>
    <fieldset>
        <div class="control-group">
            <label class="control-label" for="prependedInput">
                Prepended text</label>
            <div class="controls">
                <div class="input-prepend">
                    <span class="add-on">@</span><input id="prependedInput" size="16" type="text">
                </div>
                <p class="help-block">
                    Here's some help text</p>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="appendedInput">
                Appended text</label>
            <div class="controls">
                <div class="input-append">
                    <input id="appendedInput" size="16" type="text"><span class="add-on">.00</span>
                </div>
                <span class="help-inline">Here's more help text</span>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="appendedPrependedInput">
                Append and prepend</label>
            <div class="controls">
                <div class="input-prepend input-append">
                    <span class="add-on">$</span><input id="appendedPrependedInput" size="16" type="text"><span
                        class="add-on">.00</span>
                </div>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="appendedInputButton">
                Append with button</label>
            <div class="controls">
                <div class="input-append">
                    <input id="appendedInputButton" size="16" type="text">
                    <button class="btn" type="button">
                        Go!</button>
                </div>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label" for="appendedInputButtons">
                Two-button append</label>
            <div class="controls">
                <div class="input-append">
                    <input id="appendedInputButtons" size="16" type="text">
                    <button class="btn" type="button">
                        Search</button>
                    <button class="btn" type="button">
                        Options</button>
                </div>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label">
                Checkboxes</label>
            <div class="controls">
                <label class="checkbox inline">
                    <div id="uniform-inlineCheckbox1" class="checker">
                        <span>
                            <input style="opacity: 0;" id="inlineCheckbox1" value="option1" type="checkbox"></span></div>
                    Option 1
                </label>
                <label class="checkbox inline">
                    <div id="uniform-inlineCheckbox2" class="checker">
                        <span>
                            <input style="opacity: 0;" id="inlineCheckbox2" value="option2" type="checkbox"></span></div>
                    Option 2
                </label>
                <label class="checkbox inline">
                    <div id="uniform-inlineCheckbox3" class="checker">
                        <span>
                            <input style="opacity: 0;" id="inlineCheckbox3" value="option3" type="checkbox"></span></div>
                    Option 3
                </label>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label">
                File Upload</label>
            <div class="controls">
                <div id="uniform-undefined" class="uploader">
                    <input style="opacity: 0;" size="19" type="file"><span class="filename">No file selected</span><span
                        class="action">Choose File</span></div>
            </div>
        </div>
        <div class="control-group">
            <label class="control-label">
                Radio buttons</label>
            <div class="controls">
                <label class="radio">
                    <div id="uniform-optionsRadios1" class="radio">
                        <span class="checked">
                            <input style="opacity: 0;" id="optionsRadios1" name="optionsRadios" value="option1"
                                checked="" type="radio"></span></div>
                    Option one is this and that—be sure to include why it's great
                </label>
                <div style="clear: both;">
                </div>
                <label class="radio">
                    <div id="uniform-optionsRadios2" class="radio">
                        <span>
                            <input style="opacity: 0;" id="optionsRadios2" name="optionsRadios" value="option2"
                                type="radio"></span></div>
                    Option two can be something else and selecting it will deselect option one
                </label>
            </div>
        </div>
        <div class="form-actions">
            <button class="btn btn-primary" type="submit">
                Save changes</button>
            <button class="btn">
                Cancel</button>
        </div>
    </fieldset>
    <h1>
        h1. Heading 1</h1>
    <h2>
        h2. Heading 2</h2>
    <h3>
        h3. Heading 3</h3>
    <h4>
        h4. Heading 4</h4>
    <h5>
        h5. Heading 5</h5>
    <h6>
        h6. Heading 6</h6>
  <div class="cont_tools">
        <div class="input-prepend">
            <i class="icon-eye-open"></i><span  >用户名称:</span><input id="Text2"
                size="16" type="text" />
            <button class="btn  btn-primary" type="submit" onclick="doQuery()">
                <i class="icon-white icon-search"></i>查询
            </button>
        </div>
    </div>
</asp:Content>
