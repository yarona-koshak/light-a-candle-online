import { send } from "../utilities";

type Picture={
    Text: string;
    Src:string;
}
let pictureConteiner= document.getElementById("pictureConteiner") as HTMLDivElement;
  function showCustomAlert(message: string) {
      let alertBox = document.getElementById("custom-alert")!;
      let alertText = document.getElementById("alert-text")!;
      alertText.textContent = message;
      alertBox.style.display = "block";
    }

   (window as any).closeCustomAlert = function () {
  let alertBox = document.getElementById("custom-alert")!;
  alertBox.style.display = "none";
}
generatePreviews();


async function generatePreviews() {
   let pictures = await send("getPicture", []) as Picture[];
   console.log(typeof pictures);
    for (let p of pictures) {
        console.log(typeof p.Src);
        console.log(typeof p.Text);
        let img = document.createElement("img");
        img.classList.add("img");
        img.src = p.Src;
        pictureConteiner.appendChild(img);
    if (img) {
        img.addEventListener('click', () => {
      showCustomAlert(p.Text);
  });
} 
  }
}
