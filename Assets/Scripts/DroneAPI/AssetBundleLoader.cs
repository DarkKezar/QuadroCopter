using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssetBundleLoader : MonoBehaviour
{
    public TextMeshProUGUI progressText;

    public Slider progressBar;
    private List<string> _jokePhrases = new List<string>() {
        "Проверка скорости: готовы к космическим рекордам!",
        "Печатаем код для идеального полета — почти готово!",
        "Включаем систему стабилизации: ваш квадрик не будет качаться.",
        "Убедитесь, что не забыли ремень безопасности!",
        "Пожалуйста, пристегнитесь — мы скоро взлетаем!",
        "Прокачиваем скорость — кто сказал, что квадрики не могут летать быстро?",
        "Пора запустить двигатели… они как чайник, всегда готовы кипеть!",
        "Включаем автопилот. Не переживай, мы знаем, куда летим.",
        "Проверяем все датчики — они не любят, когда их игнорируют.",
        "Регистрируем маршрут, не переживай, не заблудимся!",
        "Загружаем новые маршруты и пару секретных маневров.",
        "Прокачиваем режим \"Летать, не теряя стиль\".",
        "Перезагружаем систему безопасности — вдруг что-то не так с этим миром.",
        "Публикуем новый тренд — воздушные акробаты!",
        "Погружаем квадрик в режим: \"Никаких задержек!\"",
        "Настроили антенны, теперь будем слышать и на Луне.",
        "Устанавливаем суперзаводную настройку — полет будет как в кино!",
        "Проверяем батареи — не хватит 10 минут, так у нас есть запасы.",
        "Загружаем уровень с крутыми поворотами — вам понравится!",
        "Прокачиваем систему стабилизации — до встречи в воздухе!",
        "Подключаем Wi-Fi к самолету. Давайте проверим сигнал.",
        "Тренируем систему навигации — она всегда в поисках приключений.",
        "Пилотируем на максимальных оборотах — а где тормоза?",
        "Идеальная погода для полетов... почти идеальная!",
        "Загружаем сверхточные координаты — держись крепче!",
        "Проверяем акселерометр — готовы взлететь в любой момент!",
        "Разгоняемся — и да пребудет с нами воздушный поток!",
        "Устанавливаем новый рекорд скорости — держись!",
        "Калибруем высоту — будем летать как птицы!",
        "Перезаряжаем импульсы — квадрик готов к действию!",
        "Включаем режим \"Невидимка\" — полет под прикрытием.",
        "Проверяем камеры — не забудь, мы тоже можем снимать!",
        "Подключаем навигацию — ориентируемся по звездам!",
        "Ожидаем зеленый свет — и вперед к новым горизонтам!",
        "Настроили стабилизацию — никаких турбуленций!",
        "Подзаряжаем батареи — не забудь про зарядку!",
        "Прокачиваем GPS-сигнал, чтобы не заблудиться.",
        "Сборка квадрокоптера: 99% готово, 1% — магия.",
        "Перезагружаем мозги квадрика.",
        "Калибруем компас — летим в правильном направлении!",
        "Протираем камеры — чтобы не было туманно.",
        "Прокачиваем антенну для суперсвязи.",
        "Проверка пропеллеров: закрутили, не расслабляемся!",
        "Стартуем с прицелом на победу!",
        "Секретная команда: \"Включить режим суперпилота\".",
        "Врубаем турбонаддув... Шутка, просто проверяем двигатель.",
        "Загружаем карту маршрута, чтоб не сесть в кусты.",
        "Включаем \"Режим Джеймса Бонда\" — будем летать как никогда!",
        "Зажигаем, как будто это последний полет!",
        "Прокачиваем датчики — теперь будем видеть всё!"
    };

    private int _currentPhrase = 0;
    private List<int> _usedPhrases = new List<int>();
    private DateTime _lastUpdate = DateTime.Now;

    private string getJokePhrase() {
        var updateTime = DateTime.Now;
        if((updateTime - _lastUpdate).TotalSeconds < 3) {
            return _jokePhrases[_currentPhrase];
        }
        
        if(_usedPhrases.Count == _jokePhrases.Count) {
            _usedPhrases = new List<int>();
        }
        _usedPhrases.Add(_currentPhrase);

        int index = UnityEngine.Random.Range(0, _jokePhrases.Count);
        while(_usedPhrases.Contains(index)) {
            index = UnityEngine.Random.Range(0, _jokePhrases.Count);
        }

        _currentPhrase = index;
        _lastUpdate = updateTime;

        return _jokePhrases[index];
    }

    private void Start()
    {
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        StartCoroutine(LoadAsyncScene(LevelManager.LevelName));
#elif PLATFORM_WEBGL
        StartCoroutine(DownloadSceneBundle(LevelManager.LevelName));
#else
        Debug.Log("Make build for webgl platform.");
#endif
    }

    public IEnumerator DownloadSceneBundle(string sceneName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, sceneName);


        Debug.Log("Loading scene from url: " + path);

        var www = UnityWebRequestAssetBundle.GetAssetBundle(path);

        Debug.Log("Download AssetBundle");
        www.SendWebRequest();

        while (!www.isDone)
        {
            float progress = www.downloadProgress;
            float downloadedData = www.downloadedBytes;

            var headers = www.GetResponseHeaders();
            long totalData = headers != null && headers.ContainsKey("Content-Length")
                ? long.Parse(headers["Content-Length"])
                : 0;


            float downloadedMB = downloadedData / (1024f * 1024f);
            float totalMB = totalData / (1024f * 1024f);


            progressBar.value = progress;
            //progressText.text = $"{downloadedMB:F2} MB / {totalMB:F2}/ MB";
            progressText.text = getJokePhrase();

            yield return null;
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(www);

            StartCoroutine(LoadAsyncSceneBundle(assetBundle));
        }
        else
        {
            Debug.Log(www.error);
            progressText.text = "При загрузке произошла ошибка, повторите попытку позже.";
            yield break;
        }
    }

    IEnumerator LoadAsyncSceneBundle(AssetBundle assetBundle)
    {
        string[] scenePath = assetBundle.GetAllScenePaths();

        Debug.Log("Start loading scene: " + scenePath[0]);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenePath[0], LoadSceneMode.Single);

        progressText.text = $"Ещё чуть чуть...";
        while (!asyncLoad.isDone)
        {
            progressBar.value = asyncLoad.progress;
            yield return null;
        }

        assetBundle.Unload(false);
    }

    IEnumerator LoadAsyncScene(string levelName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);

        progressText.text = $"Ещё чуть чуть...";
        while(!asyncLoad.isDone)
        {
            progressBar.value = asyncLoad.progress;
            yield return null;
        }
    }
}
