/* --- CONFIGURACIÓN --- */
const preguntas = [
    {
        pregunta: "¿Qué es lo que más me gusta de tu cara? (La verdad... 🙈)",
        imagen: "./assets/img/foto1.jpg", 
        audio: "./assets/audio/nonsense.mp3", 
        segundoInicio: 36, duracion: 18,
        opciones: ["Mis ojos", "Mi sonrisa", "Todo tú"],
        correcta: 2, tematica: ["🎵", "💿", "💋", "✨"]
    },
    {
        pregunta: "¿Dónde fue nuestra primera cita perfecta?",
        imagen: "./assets/img/foto2.jpg", 
        audio: "./assets/audio/just_one_day.mp3", 
        segundoInicio: 66, duracion: 12,      
        opciones: ["Cine", "Parque", "Comiendo"],
        correcta: 1, tematica: ["🍜", "🎬", "🌳", "💑"]
    },
    {
        pregunta: "¿Qué planes tengo para nuestro futuro?",
        imagen: "./assets/img/foto3.jpg", 
        audio: "./assets/audio/paper_rings.mp3", 
        segundoInicio: 36, duracion: 16,      
        opciones: ["Viajar juntos", "Adoptar gatitos", "Casarnos"],
        correcta: 2, tematica: ["💍", "✈️", "🐱", "🏠"]
    },
    {
        pregunta: "¿Qué siento cuando estoy contigo?",
        imagen: "./assets/img/foto4.jpg", 
        audio: "./assets/audio/iris.mp3", 
        segundoInicio: 61, duracion: 14,      
        opciones: ["Paz", "Que el mundo desaparece", "Hambre"],
        correcta: 1, tematica: ["❤️‍🔥", "🥺", "☁️", "✨"]
    },
    {
        pregunta: "¿Qué soy yo para ti?",
        imagen: "./assets/img/foto5.jpg", 
        audio: "./assets/audio/magic_shop.mp3", 
        segundoInicio: 65, duracion: 15,      
        opciones: ["Tu novio", "Tu Magic Shop", "Tu fan #1"],
        correcta: 1, tematica: ["🔮", "🗝️", "🛡️", "💜"]
    }
];

const fotosFinales = [
    "./assets/img/foto1.jpg", "./assets/img/foto2.jpg", 
    "./assets/img/foto3.jpg", "./assets/img/foto4.jpg", 
    "./assets/img/collage_final.png" 
];

// VARIABLES
let indiceActual = 0;
let audioPlayer = document.getElementById("musica-fondo");
let fadeInterval;
let decoracionInterval;

// INICIO
preguntas.forEach(p => { let a = new Audio(); a.src = p.audio; a.preload = "auto"; });
actualizarFondoDinamico(["❤️", "🌷", "✨"]); 

function iniciarExperiencia() {
    document.getElementById("pantalla-intro").classList.remove("activa");
    document.getElementById("pantalla-intro").classList.add("hidden");
    document.getElementById("pantalla-quiz").classList.remove("hidden");
    document.getElementById("pantalla-quiz").classList.add("activa");
    cargarPregunta();
}

function cargarPregunta() {
    if (indiceActual >= preguntas.length) { mostrarFinal(); return; }

    const data = preguntas[indiceActual];
    document.getElementById("pregunta-texto").innerText = data.pregunta;
    document.getElementById("pregunta-imagen").src = data.imagen;
    
    const contenedor = document.getElementById("opciones-container");
    contenedor.innerHTML = ""; 
    
    data.opciones.forEach((op, i) => {
        const btn = document.createElement("button");
        btn.classList.add("btn-opcion");
        btn.innerText = op;
        btn.onclick = () => verificarRespuesta(i, data.correcta, btn);
        contenedor.appendChild(btn);
    });

    gestionarCambioDeAudio(data.audio, data.segundoInicio);
    iniciarBarraTiempo(data.duracion);
    actualizarFondoDinamico(data.tematica); 
}

function verificarRespuesta(elegida, correcta, btn) {
    if (elegida === correcta) {
        btn.classList.add("correct");
        lanzarConfetiSimple();
        hacerFadeOut(() => { indiceActual++; cargarPregunta(); });
    } else {
        btn.classList.add("wrong");
        const t = btn.innerText;
        btn.innerText = "¡Nop! 🙈";
        setTimeout(() => { btn.classList.remove("wrong"); btn.innerText = t; }, 1000);
    }
}

function actualizarFondoDinamico(emojis) {
    const bg = document.getElementById('dynamic-bg');
    if(!bg) return;
    clearInterval(decoracionInterval); 
    bg.innerHTML = ""; 

    const crearElemento = () => {
        const div = document.createElement('div');
        div.classList.add('floating-item');
        if (Math.random() > 0.3) {
            div.innerText = "🌷";
            div.classList.add('anim-bouquet'); 
        } else {
            div.innerText = emojis[Math.floor(Math.random() * emojis.length)];
            div.classList.add('anim-float'); 
        }
        div.style.left = Math.random() * 100 + "vw";
        div.style.animationDuration = (Math.random() * 5 + 5) + "s"; 
        bg.appendChild(div);
        setTimeout(() => div.remove(), 10000);
    };
    decoracionInterval = setInterval(crearElemento, 600);
    for(let i=0; i<5; i++) crearElemento();
}

/* --- MOSTRAR FINAL (CORREGIDO) --- */
function mostrarFinal() {
    document.getElementById("pantalla-quiz").classList.remove("activa");
    document.getElementById("pantalla-quiz").classList.add("hidden");
    document.getElementById("pantalla-final").classList.remove("hidden");
    document.getElementById("pantalla-final").classList.add("activa");

    clearInterval(decoracionInterval);
    const bg = document.getElementById('dynamic-bg'); if(bg) bg.innerHTML = ""; 
    const corners = document.getElementById('corners'); if(corners) corners.style.display = 'none';

    // 1. ÁRBOL (Delay de seguridad)
    setTimeout(() => { iniciarAnimacionArbol(); }, 200);

    // 2. SLIDESHOW
    iniciarSlideshow();

    // 3. AUDIO TAYLOR SWIFT (98s -> 100s Fade)
    audioPlayer.src = "./assets/audio/love_story.mp3";
    audioPlayer.currentTime = 98; 
    audioPlayer.volume = 0;
    
    let playPromise = audioPlayer.play();
    if (playPromise !== undefined) {
        playPromise.then(_ => { 
            hacerFadeInLento(); 
            setTimeout(revelarPropuesta, 10000); // 10s delay para pregunta
        })
        .catch(e => { setTimeout(revelarPropuesta, 5000); });
    } else {
        setTimeout(revelarPropuesta, 5000);
    }
}

function revelarPropuesta() {
    const caja = document.getElementById("propuesta-container");
    if(caja.classList.contains("invisible")) {
        caja.classList.remove("invisible");
        caja.classList.add("visible");
        lanzarConfetiSimple();
    }
}

function iniciarSlideshow() {
    const contenedor = document.getElementById("slideshow");
    if(!contenedor) return;
    contenedor.innerHTML = ""; 
    fotosFinales.forEach((src, i) => {
        const img = document.createElement("img");
        img.src = src;
        img.classList.add("slide-foto");
        if(i === 0) img.classList.add("active");
        contenedor.appendChild(img);
    });
    let idx = 0;
    setInterval(() => {
        const slides = document.querySelectorAll(".slide-foto");
        if(slides.length>0){
            slides[idx].classList.remove("active");
            idx = (idx + 1) % slides.length;
            slides[idx].classList.add("active");
        }
    }, 3500);
}

function aceptarPropuesta() {
    lanzarConfetiGigante();
    audioPlayer.volume = 1.0; 
    setTimeout(() => { alert("¡TE AMO! Gracias por decir que sí ❤️💍\nTe amo infinitamente."); }, 1500);
}

/* --- ÁRBOL DEL AMOR (HACKEADO PARA FUNCIONAR SIEMPRE) --- */
function iniciarAnimacionArbol() {
    var canvas = $('#canvas-tree');
    if (!canvas[0] || !canvas[0].getContext) return;

    // TRUCO MAESTRO: Forzamos la resolución interna que el árbol espera
    // No importa el tamaño del celular, el árbol cree que está en una pantalla HD
    canvas.attr("width", 1100);
    canvas.attr("height", 680);
    
    // Luego, en CSS, forzamos que se estire para llenar la pantalla (ver style.css)
    canvas.css("width", "100%");
    canvas.css("height", "100%");

    // Configuración del Árbol
    var opts = {
        seed: { x: 1100 / 2 - 20, color: "rgb(190, 26, 37)", scale: 2 },
        branch: [ [535, 680, 570, 250, 500, 200, 30, 100, [ [540, 500, 455, 417, 340, 400, 13, 100, [ [450, 435, 434, 430, 394, 395, 2, 40] ]], [550, 445, 600, 356, 680, 345, 12, 100, [ [578, 400, 648, 409, 661, 426, 3, 80] ]], [539, 281, 537, 248, 534, 217, 3, 40], [546, 397, 413, 247, 328, 244, 9, 80, [ [427, 286, 383, 253, 371, 205, 2, 40], [498, 345, 435, 315, 395, 330, 4, 60] ]], [546, 357, 608, 252, 678, 221, 6, 100, [ [590, 293, 646, 277, 648, 271, 2, 80] ]] ]] ],
        bloom: { num: 700, width: 1080, height: 650 },
        footer: { width: 1200, height: 5, speed: 10 }
    }

    var tree = new Tree(canvas[0], 1100, 680, opts);
    var seed = tree.seed;
    var foot = tree.footer;
    var hold = 1;

    // Click Interactividad (Sacudir)
    canvas.click(function(e) {
        canvas.addClass("shaking");
        setTimeout(function() { canvas.removeClass("shaking"); }, 500);
        // Coordenadas relativas al escalado CSS
        // No necesitamos precisión milimétrica para las flores extra
        var newBloom = new Bloom(tree, new Point(e.offsetX || e.pageX, e.offsetY || e.pageY), tree.seed.heart.figure, 'rgb(255,0,0)', 1, null, 1, new Point(e.offsetX, 680), 300);
        tree.addBloom(newBloom);
    });

    // --- SECUENCIA AUTOMÁTICA (BYPASS SEED) ---
    // Nos saltamos la animación de la semilla cayendo porque suele fallar
    // Vamos directo a dibujar el suelo y crecer el árbol.
    var runAsync = eval(Jscex.compile("async", function () {
        $await(foot.animate()); // Suelo
        $await(tree.grow());    // Crecer
        $await(tree.flower(2)); // Flores
    }));

    runAsync().start();
}

/* --- UTILIDADES --- */
function iniciarBarraTiempo(s) { const b = document.getElementById("barra-progreso"); if(b){ b.style.transition = "none"; b.style.width = "100%"; void b.offsetWidth; b.style.transition = `width ${s}s linear`; b.style.width = "0%"; }}
function gestionarCambioDeAudio(ruta, inicio) { audioPlayer.volume = 0; audioPlayer.src = ruta; audioPlayer.currentTime = inicio; audioPlayer.play().then(hacerFadeIn).catch(e => console.log("...")); }
function hacerFadeIn() { clearInterval(fadeInterval); let vol = 0; fadeInterval = setInterval(() => { if(vol<0.8){ vol+=0.05; audioPlayer.volume=vol; } else clearInterval(fadeInterval); }, 100); }
function hacerFadeInLento() { clearInterval(fadeInterval); let vol = 0; fadeInterval = setInterval(() => { if(vol<0.95){ vol+=0.05; audioPlayer.volume=vol; } else { audioPlayer.volume = 1; clearInterval(fadeInterval); } }, 100); }
function hacerFadeOut(cb) { clearInterval(fadeInterval); let vol = audioPlayer.volume; fadeInterval = setInterval(() => { if(vol>0.05){ vol-=0.05; audioPlayer.volume=vol; } else { clearInterval(fadeInterval); audioPlayer.pause(); if(cb) cb(); } }, 100); }
function lanzarConfetiSimple() { confetti({ particleCount: 50, spread: 70, origin: { y: 0.7 } }); }

// CONFETI CORAZÓN REAL
function lanzarConfetiGigante() { 
    var end = Date.now() + 5000;
    (function frame() {
        // Scalar 4 = Corazones grandes y visibles
        confetti({ particleCount: 7, angle: 60, spread: 55, origin: { x: 0 }, shapes: ['heart'], colors: ['#ff0000', '#ff4d6d'], scalar: 4 });
        confetti({ particleCount: 7, angle: 120, spread: 55, origin: { x: 1 }, shapes: ['heart'], colors: ['#ff0000', '#ff4d6d'], scalar: 4 });
        if (Date.now() < end) requestAnimationFrame(frame);
    }()); 
}