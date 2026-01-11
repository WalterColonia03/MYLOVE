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

let indiceActual = 0;
let audioPlayer = document.getElementById("musica-fondo");
let fadeInterval;
let decoracionInterval;

/* --- INICIO --- */
// Precarga de audio
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

/* --- MOSTRAR FINAL --- */
function mostrarFinal() {
    document.getElementById("pantalla-quiz").classList.remove("activa");
    document.getElementById("pantalla-quiz").classList.add("hidden");
    document.getElementById("pantalla-final").classList.remove("hidden");
    document.getElementById("pantalla-final").classList.add("activa");

    // Limpieza
    clearInterval(decoracionInterval);
    const bg = document.getElementById('dynamic-bg'); if(bg) bg.innerHTML = ""; 
    const corners = document.getElementById('corners'); if(corners) corners.style.display = 'none';

    // 1. ÁRBOL: Iniciamos con un pequeño retraso
    setTimeout(() => {
        iniciarAnimacionArbol();
    }, 100);

    // 2. Slideshow
    iniciarSlideshow();

    // 3. Audio Final
    audioPlayer.src = "./assets/audio/love_story.mp3";
    audioPlayer.currentTime = 98; 
    audioPlayer.volume = 0;
    
    let playPromise = audioPlayer.play();
    if (playPromise !== undefined) {
        playPromise.then(_ => { 
            hacerFadeInLento(); 
            // Mostramos la pregunta después de 10 segundos
            setTimeout(revelarPropuesta, 10000); 
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
    setTimeout(() => { alert("¡TE AMO! Gracias por decir que sí ❤️💍"); }, 1500);
}

/* --- LÓGICA DEL ÁRBOL CORREGIDA (AUTOMÁTICO Y PERSISTENTE) --- */
function iniciarAnimacionArbol() {
    var canvas = $('#canvas-tree');
    
    if (!canvas[0] || !canvas[0].getContext) {
        console.error("Canvas no encontrado");
        return;
    }

    // Ajustar tamaño del canvas a la pantalla
    var width = window.innerWidth;
    var height = window.innerHeight;
    
    canvas.attr("width", width);
    canvas.attr("height", height);

    // Escalar las coordenadas del árbol proporcionalmente al tamaño de pantalla
    var scaleX = width / 1100;  // 1100 es el ancho original del diseño
    var scaleY = height / 680;  // 680 es el alto original del diseño
    var scaleFactor = Math.min(scaleX, scaleY, 1); // No escalar más grande que el original

    var opts = {
        seed: { x: width / 2, y: 50, color: "rgb(190, 26, 37)", scale: 2 },
        branch: [ 
            [535 * scaleX, 680 * scaleY, 570 * scaleX, 250 * scaleY, 500 * scaleX, 200 * scaleY, 30 * scaleFactor, 100, 
                [ 
                    [540 * scaleX, 500 * scaleY, 455 * scaleX, 417 * scaleY, 340 * scaleX, 400 * scaleY, 13 * scaleFactor, 100, 
                        [ [450 * scaleX, 435 * scaleY, 434 * scaleX, 430 * scaleY, 394 * scaleX, 395 * scaleY, 2 * scaleFactor, 40] ]
                    ], 
                    [550 * scaleX, 445 * scaleY, 600 * scaleX, 356 * scaleY, 680 * scaleX, 345 * scaleY, 12 * scaleFactor, 100, 
                        [ [578 * scaleX, 400 * scaleY, 648 * scaleX, 409 * scaleY, 661 * scaleX, 426 * scaleY, 3 * scaleFactor, 80] ]
                    ], 
                    [539 * scaleX, 281 * scaleY, 537 * scaleX, 248 * scaleY, 534 * scaleX, 217 * scaleY, 3 * scaleFactor, 40], 
                    [546 * scaleX, 397 * scaleY, 413 * scaleX, 247 * scaleY, 328 * scaleX, 244 * scaleY, 9 * scaleFactor, 80, 
                        [ 
                            [427 * scaleX, 286 * scaleY, 383 * scaleX, 253 * scaleY, 371 * scaleX, 205 * scaleY, 2 * scaleFactor, 40], 
                            [498 * scaleX, 345 * scaleY, 435 * scaleX, 315 * scaleY, 395 * scaleX, 330 * scaleY, 4 * scaleFactor, 60] 
                        ]
                    ], 
                    [546 * scaleX, 357 * scaleY, 608 * scaleX, 252 * scaleY, 678 * scaleX, 221 * scaleY, 6 * scaleFactor, 100, 
                        [ [590 * scaleX, 293 * scaleY, 646 * scaleX, 277 * scaleY, 648 * scaleX, 271 * scaleY, 2 * scaleFactor, 80] ]
                    ] 
                ]
            ] 
        ],
        bloom: { num: 700, width: width, height: height * 0.95 },
        footer: { width: width, height: 5, speed: 10 }
    };

    var tree = new Tree(canvas[0], width, height, opts);
    var seed = tree.seed;
    var foot = tree.footer;

    // Click: Sacudir y añadir flores
    canvas.click(function(e) {
        canvas.addClass("shaking");
        setTimeout(function() { canvas.removeClass("shaking"); }, 500);
        var offset = canvas.offset();
        var x = e.pageX - offset.left;
        var y = e.pageY - offset.top;
        for (var i = 0; i < 5; i++) {
            var point = new Point(x, y);
            var newBloom = new Bloom(tree, point, tree.seed.heart.figure, 'rgb(255,0,0)', 1, null, 1, new Point(x, height), 300);
            tree.addBloom(newBloom);
        }
    });

    // --- SECUENCIA AUTOMÁTICA SIN ESPERAR CLICS ---
    var runAsync = eval(Jscex.compile("async", function () {
        // Dibujar el corazón inicial (bypass de hover - no esperamos clic)
        seed.drawHeart();
        $await(Jscex.Async.sleep(300));
        
        // Animación de reducción del corazón (semilla cayendo)
        while (seed.canScale()) {
            seed.scale(0.95);
            $await(Jscex.Async.sleep(10));
        }
        
        // Semilla cae al suelo
        while (seed.canMove()) {
            seed.move(0, 2);
            foot.draw();
            $await(Jscex.Async.sleep(10));
        }

        // Crecer el árbol
        while (tree.canGrow()) {
            tree.grow();
            $await(Jscex.Async.sleep(10));
        }

        // Florecer - añadir flores gradualmente
        while (tree.bloomsCache.length > 0) {
            tree.flower(2);
            $await(Jscex.Async.sleep(10));
        }
        
        // ¡FIN! El árbol queda estático y visible.
        // NO hay ciclos de jump() o move() que borren el canvas.
        console.log("Árbol completado - quedará estático");
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

// Confeti Corazón Grande
function lanzarConfetiGigante() { 
    var end = Date.now() + 5000;
    (function frame() {
        confetti({ particleCount: 7, angle: 60, spread: 55, origin: { x: 0 }, shapes: ['heart'], colors: ['#ff0000', '#ff4d6d'], scalar: 4 });
        confetti({ particleCount: 7, angle: 120, spread: 55, origin: { x: 1 }, shapes: ['heart'], colors: ['#ff0000', '#ff4d6d'], scalar: 4 });
        if (Date.now() < end) requestAnimationFrame(frame);
    }()); 
}