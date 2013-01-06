

//加载样式---类
function addClass(elem, value) {
    if (!elem.className) {
        elem.className = value;
    } else {
        newClass = elem.className;
        newClass += " ";
        newClass += value;
        elem.className = newClass;
    }
}
//点击实现高亮显示---类
function highOnclick(elemId, tagOff, tagOff2, classCur, add_cur) {
    if (!document.getElementsByTagName) return false;
    if (!document.getElementById(elemId)) return false;
    var elemId = document.getElementById(elemId);
    var links = elemId.getElementsByTagName("a");
    for (i = 0; i < links.length; i++) {
        if (links[i].parentNode.nodeName !== tagOff && links[i].parentNode.nodeName !== tagOff2) {
            links[i].onclick = function () {
                for (n = 0; n < links.length; n++) {
                    links[n].className = "";
                }
                this.className = classCur;
                firsttagoff(elemId, tagOff, this.parentNode.parentNode, add_cur);
                this.blur();
            }
        }
    }
}
//附属点击实现高亮显示---类
function firsttagoff(elemId, tagOff, addtag, add_cur) {
    var ulitem = elemId.getElementsByTagName(addtag.nodeName);
    var tagoffitem = elemId.getElementsByTagName(tagOff);
    for (i = 0; i < tagoffitem.length; i++) {
        tagoffitem[i].firstChild.className = "";
    }
    for (j = 0; j < ulitem.length; j++) {
        if (ulitem[j].innerHTML == addtag.innerHTML) {
            tagoffitem[j].firstChild.className = add_cur;
            break;
        }
    }
}
//加载高亮显示函数
window.onload = function sidemenu() {
    highOnclick("side", "H3", "H2", "menu_cur", "h3_cur");
}
//展开/关闭
function m_id(id) {
    return document.getElementById(id);
}
function getcookie(name) {
    var cookie_start = document.cookie.indexOf(name);
    var cookie_end = document.cookie.indexOf(";", cookie_start);
    return cookie_start == -1 ? '' : unescape(document.cookie.substring(cookie_start + name.length + 1, (cookie_end > cookie_start ? cookie_end : document.cookie.length)));
}
var collapsed = getcookie('m_shutoropen');
function shutoropen(menucount) {
    var classname = m_id('menuimg_' + menucount).parentNode.className;
    if (m_id('menu_' + menucount).style.display == 'none') {
        m_id('menu_' + menucount).style.display = ''; collapsed = collapsed.replace('[' + menucount + ']', '');
        m_id('menuimg_' + menucount).src = '../images/LeftNav/bt_show.png';
    } else {
        m_id('menu_' + menucount).style.display = 'none'; collapsed += '[' + menucount + ']';
        m_id('menuimg_' + menucount).src = '../images/LeftNav/bt_block.png';
    }
}

function OpenShutManager() {
    var leftnav = document.getElementById('idx_nav');
    var titlenav = document.getElementById('nav_title');
    var samllnav = document.getElementById('nav_samll');
    var boxa = document.getElementById('box_styleA');
    var cont = document.getElementById('idx_content');
    if (leftnav.style.display != "none") {
        leftnav.style.display = "none";
        titlenav.style.display = "none";
        samllnav.style.display = "block";
        boxa.style.width = "33px";
        cont.style.paddingLeft = "1px";
        document.body.style.background = "#ffffff";
    } else {
        leftnav.style.display = "block";
        titlenav.style.display = "block";
        samllnav.style.display = "none";
        boxa.style.width = "210px";
        cont.style.paddingLeft = "208px";
        document.body.style.background = "url(../images/index_bg.gif) repeat-y #ffffff";
    }
}
//更多栏
function more_box_ch() {
    var morebox = document.getElementById('more_box');
    if (morebox.style.display != "none") {
        morebox.style.display = "none";
    } else {
        morebox.style.display = "block";
    }
}