using System;
using System.Collections.Generic;
using System.Text;

namespace HelloData.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class NewFrom
    {  
                    
                //59.#'类型type' => '标题title ,键值name ,默认value,尺寸size,鼠标事件onjs'  
                    
                //60.# 'select' => '(标题title) 选择国家|1|2|3|4|5,(键值name)sel,(默认value)0|1|2|3|4|5,  
                    
                //61formarray = array(  
                    
                //62'text'=>'普通,name,123,size=80,',  
                    
                //63'textarea'=>'详细内容,text,123456,rows=5 cols=60',  
                    
                //64'password'=>'密码,pass,,',  
                    
                //65'checkbox'=>'多选|my 1|my 2|my 3,box,0|1|2|3,',  
                    
                //66'select'=>'选择国家|中国|美国|英国|德国|月球,sel,0|1|2|3|4|5',  
                    
                //67'select1'=>'选择国家2|中国2|美国2|英国2|德国2|月球2,sel2,0|1|2|3|4|5',  
                    
                //68'select3'=>'选择国家3|中国3|美国3|英国3|德国3|月球3,sel3,0|1|2|3|4|5',  
                    
                //69'radio'=>'单选|1|2|3|4,rad,0|1|2|3|4,10',  
                    
                //70'radio1'=>'单选2,rad1,1',  
                    

        /// <summary>
        /// 生成表单 Froms this instance.
        /// </summary>
        /// <returns></returns>
        public static string From(string type, string title, string name, string value, string size, string onjs)
        {
            string TEX = "";
            switch (type)
            {
                case "text":
                    TEX = form_textstring(title, name, value, size, onjs);
                    break;
                case "password":
                    TEX = form_password(title, name, value, size, onjs);
                    break;
                case "textarea":
                    TEX = form_textarea(title, name, value, size, onjs);
                    break;
                case "hidden":
                    TEX = form_hidden(title, name, value, size, onjs);
                    break;
            }
            return TEX;
        }

        public static string form_textstring(string title, string name, string value, string size, string onjs)
        {
            return "<label>" + title + "<input type='text' name='" + name + "' id='" + name + "' size='" + size + "' value='" + value + "' " + onjs + " /></label>";
        }

        public static string form_password(string title, string name, string value, string size, string onjs)
        {
            return "<label>" + title + "<input type='password' name='" + name + "' id='" + name + "' size='" + size + "' value='" + value + "' " + onjs + " /></label>";
        }

        public static string form_checkbox(string title, string name, string value, string size, string onjs)
        {
            return "";
        }

        public static string form_textarea(string title, string name, string value, string size, string onjs)
        {
            return "<textarea name='" + name + "' cols='" + size.Split('|')[0] + "' rows='" + size.Split('|')[1] + "' id='" + name + "' " + onjs + ">" + value + "</textarea>";
        }

        public static string form_hidden(string title, string name, string value, string size, string onjs)
        {
            return "<input type='hiddenField' name='" + name + "' id='" + name + "' size='" + size + "' value='" + value + "' " + onjs + " />";
        }

        public static string form_radio(string title, string name, string value, string size, string onjs)
        {
            return "";
        }
    }
}
