import flet as ft
from flet_core.control_event import ControlEvent
import main as mn
from time import sleep
import pythoncom


def main(page: ft.Page):
    page.title = "VALORez"
    page.padding = 0
    page.window_opacity = 0.98
    version = "1.0"

    page.theme_mode = 'system'

    page.snack_bar = ft.SnackBar(
        content=ft.Text(mn.output_var),
        duration=1000,
    )

    page.fonts = {
        "VALORANT": r".\assets\Valorant Font.ttf",
        "Kanit": "https://raw.githubusercontent.com/google/fonts/master/ofl/kanit/Kanit-Bold.ttf",
    }

    page.window_width = 400
    page.window_height = 500

    page.window_resizable = False
    page.window_maximizable = False
    page.window_minimizable = False

    page.window_center()
    page.window_to_front()

    page.update()

    # ===================================================

    def closing(e: ControlEvent):
        page.window_destroy()

    def startvalorant(e: ControlEvent):
        pythoncom.CoInitialize()
        mn.parse()
        animate_opacityp()
        mn.execute_valorant()
        animate_opacityp()
        page.snack_bar = ft.SnackBar(content=ft.Text(mn.output_var), duration=1000)
        page.snack_bar.open = True
        page.update()

    def defaultres(e: ControlEvent):
        mn.set_resolution_default()
        dd.value = 0
        sleep(0.25)
        page.snack_bar = ft.SnackBar(content=ft.Text(mn.output_var), duration=1000)
        page.snack_bar.open = True
        page.update()

    def cleardropdown(e: ControlEvent):
        dd.value = 0
        mn.output_var = 'Input cleared.'
        page.snack_bar = ft.SnackBar(content=ft.Text(mn.output_var), duration=1000)
        page.snack_bar.open = True
        page.update()

    def confirmres(e: ControlEvent):
        try:
            if dd.value != 0 and dd.value is not None:
                mn.execute_stretch()
            width = ''
            height = ''
            i = 0
            for char in dd.value:
                if char != ' ':
                    i += 1
                    width += char
                else:
                    break
            try:
                for char in dd.value[i + 3:]:
                    height += char
            except:
                pass
            mn.set_resolution(int(width), int(height))
            mn.output_var = "VALORANT in now stretched!"
        except:
            mn.output_var = "VALORANT isn't started!"
        if dd.value == 0 or dd.value is None:
            mn.output_var = "Please select your resolution."
        dd.value = 0
        page.snack_bar = ft.SnackBar(content=ft.Text(mn.output_var), duration=1000)
        page.snack_bar.open = True
        page.update()

    def animate_opacityc():
        c.opacity = 1 if c.opacity == 0 else 0
        c.update()

    def animate_opacityp():
        processbar.opacity = 1 if processbar.opacity == 0 else 0
        processbar.update()

    title = ft.Text(
        "VALORez",
        color="red700",
        size=46,
        font_family='Kanit',
        text_align=ft.TextAlign.CENTER,
        width=page.window_width,
    )

    dd = ft.Dropdown(
        width=175,
        suffix_icon=ft.icons.SCREENSHOT_MONITOR,
        content_padding=7,
        border_radius=20,
        focused_border_color='white',
        hint_text=" ____ x ____",
        options=[
        ],
    )

    confirm = ft.IconButton(
        icon=ft.icons.CHECK,
        icon_color='green',
        on_click=confirmres,
    )

    default = ft.IconButton(
        icon=ft.icons.RESET_TV,
        icon_color='red100',
        on_click=defaultres,
    )

    clear = ft.IconButton(
        icon=ft.icons.DELETE,
        icon_color='red100',
        on_click=cleardropdown,
    )

    start = ft.ElevatedButton(
        text="Start & Patch VALORANT",
        height=40,
        width=page.window_width - 75,
        on_click=startvalorant,
        style=ft.ButtonStyle(
            color='red100',
            animation_duration=1000,
            bgcolor={ft.MaterialState.DEFAULT: page.bgcolor, ft.MaterialState.PRESSED: 'red700'},
            shape={
                ft.MaterialState.HOVERED: ft.RoundedRectangleBorder(radius=90),
                ft.MaterialState.DEFAULT: ft.RoundedRectangleBorder(radius=5),
            },
        ),
    )

    close = ft.ElevatedButton(
        text="Close",
        height=40,
        width=page.window_width - 75,
        on_click=closing,
        style=ft.ButtonStyle(
            color='red300',
            animation_duration=1000,
            bgcolor={ft.MaterialState.DEFAULT: page.bgcolor, ft.MaterialState.PRESSED: 'red700'},
            shape={
                ft.MaterialState.HOVERED: ft.RoundedRectangleBorder(radius=90),
                ft.MaterialState.DEFAULT: ft.RoundedRectangleBorder(radius=5),
            },
        ),
    )

    r_firstline = ft.Row(
        [
            start,
        ],
        alignment=ft.MainAxisAlignment.CENTER
    )

    r_resolution = ft.Row(
        [
            dd,
            confirm,
            default,
            clear,
        ],
        alignment=ft.MainAxisAlignment.CENTER
    )

    r_close = ft.Row(
        [
            close
        ],
        alignment=ft.MainAxisAlignment.CENTER
    )

    contitle = ft.Container(
        content=title,
        padding=ft.padding.only(top=30),
    )

    con = ft.Container(
        content=r_firstline,
        padding=15
    )

    conclose = ft.Container(
        content=r_close,
        padding=ft.padding.only(top=15, bottom=100),
    )

    processbar = ft.Container(
        content=ft.ProgressBar(
            width=400,
            color="red700",
            bgcolor=page.bgcolor,
        ),
        width=page.window_width,
        animate_opacity=100,
    )

    c = ft.Container(
        content=ft.Column(
            [
                contitle,
                con,
                r_resolution,
                conclose,
                ft.Text(f"           version: {version} - NEM3S", size=10, opacity=0.5)
            ]
        ),
        height=447,
        animate_opacity=0,
    )

    page.add(c, processbar)

    confirm.disabled = True
    close.disabled = True
    default.disabled = True
    clear.disabled = True
    start.disabled = True
    animate_opacityc()
    c.animate_opacity = 250
    sleep(1.2)
    confirm.disabled = False
    close.disabled = False
    default.disabled = False
    clear.disabled = False
    start.disabled = False
    listsorted = mn.get_all_resolutions()
    listsorted.sort(reverse=True)
    for i in range(len(listsorted)):
        dd.options.append(ft.dropdown.Option('{} x {}'.format(listsorted[i][0], listsorted[i][1])))
        page.update()
    animate_opacityp()
    animate_opacityc()


if __name__ == '__main__':
    ft.app(target=main)