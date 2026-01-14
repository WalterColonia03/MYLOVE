/* === CONFIGURACIÓN === */
const preguntas = [
    {
        pregunta: "¿Qué es lo que más me gusta de tu cara? 🙈❤️",
        imagen: "./assets/img/foto1.jpg",
        audio: "./assets/audio/nonsense.mp3",
        segundoInicio: 36, duracion: 18,
        opciones: ["Mis ojos ✨", "Mi sonrisa 😊", "Todo es perfecto 💫"],
        correcta: 2,
        tematica: ["❤️", "🌷", "✨", "🌸"],
        mensajeCorrecto: "¡Exacto! Todo tú me encanta ❤️"
    },
    {
        pregunta: "¿Dónde fue nuestra primera cita perfecta? 🍜",
        imagen: "./assets/img/foto2.jpg",
        audio: "./assets/audio/just_one_day.mp3",
        segundoInicio: 66, duracion: 12,
        opciones: ["En el cine 🎬", "En el parque 🌳", "Comiendo juntos 🍝"],
        correcta: 2,
        tematica: ["🍜", "❤️", "💑", "🌷"],
        mensajeCorrecto: "¡Los fideos siempre nos unen! 🍜"
    },
    {
        pregunta: "¿Qué planes tengo para nuestro futuro? 💍",
        imagen: "./assets/img/foto3.jpg",
        audio: "./assets/audio/paper_rings.mp3",
        segundoInicio: 36, duracion: 16,
        opciones: ["Viajar por el mundo ✈️", "Tener gatitos 🐱", "Casarnos algún día 💒"],
        correcta: 2,
        tematica: ["💍", "❤️", "🏠", "✨"],
        mensajeCorrecto: "Paper Rings 💍 - Taylor lo sabía"
    },
    {
        pregunta: "¿Qué siento cuando estoy contigo? 🌅",
        imagen: "./assets/img/foto4.jpg",
        audio: "./assets/audio/iris.mp3",
        segundoInicio: 61, duracion: 14,
        opciones: ["Paz infinita 🕊️", "Que el mundo desaparece 💫", "Mariposas 🦋"],
        correcta: 1,
        tematica: ["❤️", "✨", "🌙", "💫"],
        mensajeCorrecto: "Cuando estamos juntos, solo existimos tú y yo ❤️"
    },
    {
        pregunta: "¿Qué soy yo para ti? ❤️",
        imagen: "./assets/img/foto5.jpg",
        audio: "./assets/audio/magic_shop.mp3",
        segundoInicio: 65, duracion: 15,
        opciones: ["Tu mejor amigo 👫", "Tu Magic Shop 🔮", "Tu fan #1 de K-pop 🎤"],
        correcta: 1,
        tematica: ["🔮", "❤️", "🗝️", "✨"],
        mensajeCorrecto: "Siempre seré tu Magic Shop 🔮❤️"
    }
];

const fotosFinales = ["./assets/img/foto_final.png"];

let indiceActual = 0;
let audioPlayer = document.getElementById("musica-fondo");
let fadeInterval;
let decoracionInterval;

/* === INICIO === */
preguntas.forEach(p => {
    let a = new Audio();
    a.src = p.audio;
    a.preload = "auto";
});

// Generar estrellas aleatorias al cargar
generarEstrellasAleatorias();
// Generar gaviotas con movimiento irregular
generarGaviotasIrregulares();

actualizarFondoDinamico(["❤️", "🌷", "✨", "💖"]);

/* === ESTRELLAS ALEATORIAS === */
function generarEstrellasAleatorias() {
    const container = document.getElementById('stars-container');
    if (!container) return;
    container.innerHTML = '';

    const numEstrellas = 15 + Math.floor(Math.random() * 10); // 15-25 estrellas

    for (let i = 0; i < numEstrellas; i++) {
        const star = document.createElement('div');
        star.className = 'star';
        star.style.left = Math.random() * 100 + '%';
        star.style.top = Math.random() * 40 + '%'; // Solo en parte superior
        star.style.width = (2 + Math.random() * 3) + 'px';
        star.style.height = star.style.width;
        star.style.animationDelay = Math.random() * 3 + 's';
        star.style.animationDuration = (2 + Math.random() * 2) + 's';
        container.appendChild(star);
    }

    // Regenerar cada 30 segundos para variedad
    setTimeout(generarEstrellasAleatorias, 30000);
}

/* === GAVIOTAS CON MOVIMIENTO IRREGULAR === */
function generarGaviotasIrregulares() {
    const container = document.getElementById('seagulls-container');
    if (!container) return;

    function crearGaviota() {
        const gaviota = document.createElement('span');
        gaviota.className = 'seagull';
        gaviota.textContent = '⌒'; // Forma de gaviota

        // Posición inicial aleatoria
        const startFromLeft = Math.random() > 0.5;
        gaviota.style.left = startFromLeft ? '-5%' : '105%';
        gaviota.style.top = (5 + Math.random() * 30) + '%';
        gaviota.style.fontSize = (0.8 + Math.random() * 0.8) + 'rem';
        gaviota.style.transform = startFromLeft ? 'scaleX(1)' : 'scaleX(-1)';

        container.appendChild(gaviota);

        // Animación manual con dirección variable
        const direccion = startFromLeft ? 1 : -1;
        const velocidad = 0.05 + Math.random() * 0.1;
        const amplitud = 10 + Math.random() * 20;
        let posX = startFromLeft ? -5 : 105;
        let tiempo = 0;

        function mover() {
            tiempo += 0.02;
            posX += velocidad * direccion;
            const posY = parseFloat(gaviota.style.top) + Math.sin(tiempo * 2) * 0.1;

            gaviota.style.left = posX + '%';
            gaviota.style.top = posY + '%';

            // Pequeño aleteo
            const aleteo = Math.sin(tiempo * 10) * 5;
            gaviota.style.transform = `scaleX(${direccion}) rotate(${aleteo}deg)`;

            if (posX > -10 && posX < 110) {
                requestAnimationFrame(mover);
            } else {
                gaviota.remove();
            }
        }

        mover();
    }

    // Crear gaviotas a intervalos irregulares
    function programarGaviota() {
        crearGaviota();
        const siguiente = 3000 + Math.random() * 5000; // 3-8 segundos
        setTimeout(programarGaviota, siguiente);
    }

    // Iniciar con algunas gaviotas
    programarGaviota();
    setTimeout(programarGaviota, 2000);
}

function iniciarExperiencia() {
    const intro = document.getElementById("pantalla-intro");
    intro.style.opacity = "0";
    intro.style.transform = "scale(0.95)";

    setTimeout(() => {
        intro.classList.remove("activa");
        intro.classList.add("hidden");

        // Iniciar el juego HOG de BTS en lugar del quiz
        document.getElementById("pantalla-hog").classList.remove("hidden");
        document.getElementById("pantalla-hog").classList.add("activa");
        iniciarJuegoHOG();
    }, 500);

    lanzarConfetiBienvenida();
}

function cargarPregunta() {
    if (indiceActual >= preguntas.length) {
        mostrarFinal();
        return;
    }

    const data = preguntas[indiceActual];

    const preguntaTexto = document.getElementById("pregunta-texto");
    preguntaTexto.style.opacity = "0";
    preguntaTexto.style.transform = "translateY(-20px)";

    setTimeout(() => {
        preguntaTexto.innerText = data.pregunta;
        preguntaTexto.style.transition = "all 0.5s ease";
        preguntaTexto.style.opacity = "1";
        preguntaTexto.style.transform = "translateY(0)";
    }, 100);

    document.getElementById("pregunta-imagen").src = data.imagen;

    const contenedor = document.getElementById("opciones-container");
    contenedor.innerHTML = "";

    data.opciones.forEach((op, i) => {
        const btn = document.createElement("button");
        btn.classList.add("btn-opcion");
        btn.style.opacity = "0";
        btn.style.transform = "translateX(-30px)";
        btn.innerText = op;
        btn.onclick = () => verificarRespuesta(i, data.correcta, btn, data.mensajeCorrecto);
        contenedor.appendChild(btn);

        setTimeout(() => {
            btn.style.transition = "all 0.4s ease";
            btn.style.opacity = "1";
            btn.style.transform = "translateX(0)";
        }, 200 + (i * 150));
    });

    gestionarCambioDeAudio(data.audio, data.segundoInicio);
    iniciarBarraTiempo(data.duracion);
    actualizarFondoDinamico(data.tematica);
}

function verificarRespuesta(elegida, correcta, btn, mensajeCorrecto) {
    if (elegida === correcta) {
        btn.classList.add("correct");
        lanzarConfetiCelebracion();

        const toast = crearToastMensaje(mensajeCorrecto || "¡Correcto! ❤️");
        document.body.appendChild(toast);
        setTimeout(() => toast.remove(), 2500);

        setTimeout(() => {
            hacerFadeOut(() => {
                indiceActual++;
                cargarPregunta();
            });
        }, 1500);
    } else {
        btn.classList.add("wrong");
        const t = btn.innerText;
        btn.innerText = "¡Intenta otra vez! 🙈";

        if (navigator.vibrate) navigator.vibrate(100);

        setTimeout(() => {
            btn.classList.remove("wrong");
            btn.innerText = t;
        }, 1200);
    }
}

function crearToastMensaje(mensaje) {
    const toast = document.createElement("div");
    toast.style.cssText = `
        position: fixed;
        top: 20%;
        left: 50%;
        transform: translateX(-50%);
        background: linear-gradient(135deg, rgba(220, 20, 60, 0.95), rgba(232, 125, 78, 0.9));
        color: white;
        padding: 20px 30px;
        border-radius: 30px;
        font-size: 1.2rem;
        font-weight: 600;
        z-index: 9999;
        text-align: center;
        box-shadow: 0 10px 40px rgba(0,0,0,0.4);
        animation: toastIn 0.5s ease forwards;
        border: 2px solid rgba(255,255,255,0.3);
    `;
    toast.innerText = mensaje;

    const style = document.createElement("style");
    style.textContent = `
        @keyframes toastIn {
            0% { opacity: 0; transform: translateX(-50%) translateY(-30px) scale(0.8); }
            100% { opacity: 1; transform: translateX(-50%) translateY(0) scale(1); }
        }
    `;
    document.head.appendChild(style);
    setTimeout(() => style.remove(), 3000);

    return toast;
}

function actualizarFondoDinamico(emojis) {
    const bg = document.getElementById('dynamic-bg');
    if (!bg) return;
    clearInterval(decoracionInterval);

    const tulipanes = ["🌷", "💐", "🌸", "🌺"];
    const corazones = ["❤️", "💖", "💕", "💗"];
    const estrellas = ["✨", "⭐", "💫", "🌟"];

    const crearElemento = () => {
        const div = document.createElement('div');
        div.classList.add('floating-item');

        const rand = Math.random();

        if (rand > 0.7) {
            div.innerText = tulipanes[Math.floor(Math.random() * tulipanes.length)];
            div.classList.add('anim-bouquet');
            div.style.fontSize = (2 + Math.random() * 1.5) + "rem";
        } else if (rand > 0.45) {
            div.innerText = corazones[Math.floor(Math.random() * corazones.length)];
            div.classList.add('anim-heart');
            div.style.fontSize = (1.8 + Math.random() * 1.2) + "rem";
        } else if (rand > 0.25) {
            const todosEmojis = [...emojis];
            div.innerText = todosEmojis[Math.floor(Math.random() * todosEmojis.length)];
            div.classList.add('anim-float');
        } else {
            div.innerText = estrellas[Math.floor(Math.random() * estrellas.length)];
            div.classList.add('anim-sparkle');
            div.style.fontSize = (1.5 + Math.random()) + "rem";
        }

        div.style.left = Math.random() * 100 + "vw";
        div.style.animationDuration = (10 + Math.random() * 8) + "s";
        bg.appendChild(div);
        setTimeout(() => div.remove(), 18000);
    };

    decoracionInterval = setInterval(crearElemento, 700);
    for (let i = 0; i < 10; i++) crearElemento();
}

/* === PANTALLA FINAL === */
function mostrarFinal() {
    const quiz = document.getElementById("pantalla-quiz");
    quiz.style.opacity = "0";

    setTimeout(() => {
        quiz.classList.remove("activa");
        quiz.classList.add("hidden");
        document.getElementById("pantalla-final").classList.remove("hidden");
        document.getElementById("pantalla-final").classList.add("activa");

        clearInterval(decoracionInterval);
        const bg = document.getElementById('dynamic-bg');
        if (bg) bg.innerHTML = "";
        const corners = document.getElementById('corners');
        if (corners) corners.style.display = 'none';

        setTimeout(() => {
            iniciarAnimacionArbol();
        }, 300);

        audioPlayer.src = "./assets/audio/love_story.mp3";
        audioPlayer.currentTime = 98;
        audioPlayer.volume = 0;

        let playPromise = audioPlayer.play();
        if (playPromise !== undefined) {
            playPromise.then(_ => {
                hacerFadeInLento();
                setTimeout(revelarPropuesta, 10000);
            })
                .catch(e => {
                    console.log("Audio autoplay prevented");
                    setTimeout(revelarPropuesta, 5000);
                });
        } else {
            setTimeout(revelarPropuesta, 5000);
        }
    }, 500);
}

function revelarPropuesta() {
    const caja = document.getElementById("propuesta-container");
    if (caja.classList.contains("invisible")) {
        caja.classList.remove("invisible");
        caja.classList.add("visible");
        lanzarConfetiCelebracion();

        if (navigator.vibrate) {
            navigator.vibrate([100, 50, 100, 50, 200]);
        }
    }
}

function aceptarPropuesta() {
    lanzarConfetiGigante();
    audioPlayer.volume = 1.0;

    if (navigator.vibrate) {
        navigator.vibrate([200, 100, 200, 100, 400]);
    }

    setTimeout(() => {
        alert("¡TE AMO MUCHO! Gracias por decir que sí ❤️💍\n\nEres lo mejor que me ha pasado ✨");
    }, 1500);
}

/* === ÁRBOL - SOLO SACUDIR, NO CREAR NUEVOS ELEMENTOS === */
function iniciarAnimacionArbol() {
    var canvas = $('#canvas-tree');

    if (!canvas[0] || !canvas[0].getContext) {
        console.error("Canvas no encontrado");
        return;
    }

    var canvasWidth = canvas.width();
    var canvasHeight = canvas.height();

    canvas.attr("width", canvasWidth);
    canvas.attr("height", canvasHeight);

    // Escalar y CENTRAR el árbol correctamente
    var scaleX = canvasWidth / 1100;
    var scaleY = canvasHeight / 680;
    var scaleFactor = Math.min(scaleX, scaleY, 1);

    // Centro del canvas
    var centerX = canvasWidth / 2;

    var opts = {
        seed: { x: centerX, y: 50, color: "rgb(114, 47, 55)", scale: 2 },
        branch: [
            // Tronco principal centrado
            [centerX, canvasHeight - 10, centerX + 30 * scaleFactor, 250 * scaleY, centerX - 30 * scaleFactor, 200 * scaleY, 25 * scaleFactor, 100,
                [
                    // Rama izquierda
                    [centerX, 450 * scaleY, centerX - 100 * scaleFactor, 380 * scaleY, centerX - 180 * scaleFactor, 350 * scaleY, 12 * scaleFactor, 100,
                        [[centerX - 120 * scaleFactor, 380 * scaleY, centerX - 150 * scaleFactor, 360 * scaleY, centerX - 180 * scaleFactor, 340 * scaleY, 3 * scaleFactor, 50]]
                    ],
                    // Rama derecha
                    [centerX, 450 * scaleY, centerX + 100 * scaleFactor, 380 * scaleY, centerX + 180 * scaleFactor, 350 * scaleY, 12 * scaleFactor, 100,
                        [[centerX + 120 * scaleFactor, 380 * scaleY, centerX + 150 * scaleFactor, 360 * scaleY, centerX + 180 * scaleFactor, 340 * scaleY, 3 * scaleFactor, 50]]
                    ],
                    // Rama superior central
                    [centerX, 280 * scaleY, centerX, 220 * scaleY, centerX, 180 * scaleY, 4 * scaleFactor, 40],
                    // Rama izquierda superior
                    [centerX - 20 * scaleFactor, 350 * scaleY, centerX - 130 * scaleFactor, 250 * scaleY, centerX - 180 * scaleFactor, 220 * scaleY, 8 * scaleFactor, 80,
                    [
                        [centerX - 100 * scaleFactor, 280 * scaleY, centerX - 140 * scaleFactor, 250 * scaleY, centerX - 160 * scaleFactor, 200 * scaleY, 3 * scaleFactor, 40]
                    ]
                    ],
                    // Rama derecha superior
                    [centerX + 20 * scaleFactor, 350 * scaleY, centerX + 130 * scaleFactor, 250 * scaleY, centerX + 180 * scaleFactor, 220 * scaleY, 8 * scaleFactor, 80,
                    [[centerX + 100 * scaleFactor, 280 * scaleY, centerX + 140 * scaleFactor, 250 * scaleY, centerX + 160 * scaleFactor, 200 * scaleY, 3 * scaleFactor, 40]]
                    ]
                ]
            ]
        ],
        bloom: { num: 400, width: canvasWidth, height: canvasHeight * 0.7 },
        footer: { width: canvasWidth * 0.6, height: 5, speed: 10 }
    };

    var tree = new Tree(canvas[0], canvasWidth, canvasHeight, opts);
    var seed = tree.seed;
    var foot = tree.footer;

    window.loveTree = tree;

    // === CLICK: SOLO SACUDIR, NO CREAR ELEMENTOS NUEVOS ===
    canvas.on('click touchstart', function (e) {
        e.preventDefault();

        // Solo efecto de sacudida visual
        canvas.addClass("shaking");
        setTimeout(function () { canvas.removeClass("shaking"); }, 500);

        // Pequeño confeti sin crear elementos en el canvas
        var offset = canvas.offset();
        var x = e.type === 'touchstart' && e.originalEvent.touches ?
            e.originalEvent.touches[0].pageX : e.pageX;
        var y = e.type === 'touchstart' && e.originalEvent.touches ?
            e.originalEvent.touches[0].pageY : e.pageY;

        confetti({
            particleCount: 8,
            spread: 40,
            origin: { x: x / window.innerWidth, y: y / window.innerHeight },
            colors: ['#e07a5f', '#c84c09', '#d4a845', '#722f37']
        });
    });

    // === ANIMACIÓN DEL ÁRBOL ===
    var runAsync = eval(Jscex.compile("async", function () {
        seed.drawHeart();
        $await(Jscex.Async.sleep(300));

        while (seed.canScale()) {
            seed.scale(0.95);
            $await(Jscex.Async.sleep(10));
        }

        while (seed.canMove()) {
            seed.move(0, 2);
            foot.draw();
            $await(Jscex.Async.sleep(10));
        }

        while (tree.canGrow()) {
            tree.grow();
            $await(Jscex.Async.sleep(10));
        }

        while (tree.bloomsCache.length > 0) {
            tree.flower(2);
            $await(Jscex.Async.sleep(10));
        }

        console.log("Árbol completado ❤️");
    }));

    runAsync().start();
}

/* === UTILIDADES === */
function iniciarBarraTiempo(s) {
    const b = document.getElementById("barra-progreso");
    if (b) {
        b.style.transition = "none";
        b.style.width = "100%";
        void b.offsetWidth;
        b.style.transition = `width ${s}s linear`;
        b.style.width = "0%";
    }
}

function gestionarCambioDeAudio(ruta, inicio) {
    audioPlayer.volume = 0;
    audioPlayer.src = ruta;
    audioPlayer.currentTime = inicio;
    audioPlayer.play().then(hacerFadeIn).catch(e => console.log("Audio blocked"));
}

function hacerFadeIn() {
    clearInterval(fadeInterval);
    let vol = 0;
    fadeInterval = setInterval(() => {
        if (vol < 0.8) { vol += 0.05; audioPlayer.volume = vol; }
        else clearInterval(fadeInterval);
    }, 100);
}

function hacerFadeInLento() {
    clearInterval(fadeInterval);
    let vol = 0;
    fadeInterval = setInterval(() => {
        if (vol < 0.95) { vol += 0.03; audioPlayer.volume = vol; }
        else { audioPlayer.volume = 1; clearInterval(fadeInterval); }
    }, 150);
}

function hacerFadeOut(cb) {
    clearInterval(fadeInterval);
    let vol = audioPlayer.volume;
    fadeInterval = setInterval(() => {
        if (vol > 0.05) { vol -= 0.05; audioPlayer.volume = vol; }
        else { clearInterval(fadeInterval); audioPlayer.pause(); if (cb) cb(); }
    }, 100);
}

/* === CONFETI === */
function lanzarConfetiBienvenida() {
    confetti({
        particleCount: 80, spread: 100, origin: { y: 0.6 },
        colors: ['#dc143c', '#c94c7e', '#f5b041', '#e87d4e']
    });
}

function lanzarConfetiCelebracion() {
    confetti({
        particleCount: 60, spread: 80, origin: { y: 0.65 },
        colors: ['#dc143c', '#c94c7e', '#f5b041', '#e87d4e', '#d4a845']
    });
}

function lanzarConfetiGigante() {
    var end = Date.now() + 6000;
    var colors = ['#dc143c', '#c94c7e', '#f5b041', '#e87d4e', '#d4a845'];

    (function frame() {
        confetti({
            particleCount: 8, angle: 60, spread: 65, origin: { x: 0 },
            shapes: ['heart', 'circle'], colors: colors, scalar: 2.5
        });
        confetti({
            particleCount: 8, angle: 120, spread: 65, origin: { x: 1 },
            shapes: ['heart', 'circle'], colors: colors, scalar: 2.5
        });
        if (Date.now() < end) requestAnimationFrame(frame);
    }());
}